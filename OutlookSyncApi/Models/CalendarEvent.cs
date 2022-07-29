using Microsoft.Graph;

namespace OutlookSyncApi.Models
{
    public class CalendarEvent
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string EventOrganizer { get; set; }
        public DateTimeTimeZone Start { get; set; }
        public DateTimeTimeZone End { get; set; }
    }
}

