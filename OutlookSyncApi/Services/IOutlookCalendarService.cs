using System.Collections.Generic;
using System.Threading.Tasks;
using OutlookSyncApi.Models;

namespace OutlookSyncApi.Services
{
    public interface IOutlookCalendarService
    {
        Task<List<CalendarEvent>> GetCalendarEvents();
        Task CreateCalendarEvent(CalendarEvent evt);
        Task UpdateCalendarEvent(CalendarEvent evt);
        Task DeleteCalendarEvent(string id);
    }
}

