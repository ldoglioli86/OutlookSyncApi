using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OutlookSyncApi.Models;
using Microsoft.Graph;
using TimeZoneConverter;

namespace OutlookSyncApi.Services
{
    public class OutlookCalendarService : IOutlookCalendarService
    {
        private readonly GraphServiceClient graphServiceClient;
        private readonly int _numberCalendarDays;

        public OutlookCalendarService(IAuthenticationProvider authenticationProvider)
        {
            graphServiceClient = new GraphServiceClient(authenticationProvider);
            _numberCalendarDays = 31;
        }

        public async Task<List<CalendarEvent>> GetCalendarEvents()
        {

            var user = await graphServiceClient.Me.Request()
                     .Select(u => new {
                         u.DisplayName,
                         u.MailboxSettings
                     })
                     .GetAsync(); ;

            var events = await ListCalendarEvents(user.MailboxSettings.TimeZone,
                Convert.ToInt16(_numberCalendarDays));

            List<CalendarEvent> outlookCalendarEvents = new
                List<CalendarEvent>();

            foreach (Event item in events)
            {
                outlookCalendarEvents.Add(new CalendarEvent()
                {
                    Id = item.Id,
                    EventOrganizer = item.Organizer.EmailAddress.Name,
                    Subject = item.Subject,
                    Start = item.Start,
                    End = item.End
                });
            }
            return outlookCalendarEvents;
        }

        private async Task<IEnumerable<Event>> ListCalendarEvents(
            string timeZone,
            int numberOfDays)
        {
            DateTime startDate = DateTime.Now;
            if (numberOfDays > 31)
                return null;

            var start = GetUtcStartOfWeekInTimeZone(startDate, timeZone);
            var end = start.AddDays(numberOfDays);

            var viewOptions = new List<QueryOption>
            {
                new QueryOption("startDateTime", start.ToString("o")),
                new QueryOption("endDateTime", end.ToString("o"))
            };

            try
            {
                var events = await graphServiceClient.Me
                    .CalendarView
                    .Request(viewOptions)
                    .Header("Prefer", $"outlook.timezone=\"{timeZone}\"")
                    .Top(50)
                    .Select(e => new
                    {
                        e.Subject,
                        e.Organizer,
                        e.Start,
                        e.End
                    })
                    .OrderBy("start/dateTime")
                    .GetAsync();

                return events.CurrentPage;
            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"Error getting events: {ex.Message}");
                return null;
            }
        }

        private static DateTime GetUtcStartOfWeekInTimeZone(DateTime today,
             string timeZoneId)
        {
            TimeZoneInfo userTimeZone = TZConvert.GetTimeZoneInfo(timeZoneId);
            int diff = System.DayOfWeek.Sunday - today.DayOfWeek;
            var unspecifiedStart = DateTime.SpecifyKind(
                today.AddDays(diff), DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(unspecifiedStart, userTimeZone);
        }

        public async Task CreateCalendarEvent(CalendarEvent evt)
        {
            Event graphEvt = new Event
            {
                Subject = evt.Subject,
                Start = evt.Start,
                End = evt.End
            };

            try
            {
                await graphServiceClient.Me
                    .Events
                    .Request()
                    .AddAsync(graphEvt);
            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"Error creating event: {ex.Message}");
            }
        }

        public async Task UpdateCalendarEvent(CalendarEvent evt)
        {
            Event graphEvt = new Event
            {
                Id = evt.Id,
                Subject = evt.Subject,
                Start = evt.Start,
                End = evt.End
            };

            try
            {
                await graphServiceClient.Me
                    .Events[evt.Id]
                    .Request()
                    .UpdateAsync(graphEvt);
            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"Error updating event: {ex.Message}");
            }
        }

        public async Task DeleteCalendarEvent(string id)
        {
            try
            {
                await graphServiceClient.Me
                    .Events[id]
                    .Request()
                    .DeleteAsync();
            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"Error deleting event: {ex.Message}");
            }
        }
    }
}

