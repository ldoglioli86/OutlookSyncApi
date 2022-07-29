using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OutlookSyncApi.Models;
using OutlookSyncApi.Services;

namespace OutlookSyncApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OutlookCalendarController : ControllerBase
    {
        private readonly IOutlookCalendarService outlookCalendar;

        public OutlookCalendarController(IOutlookCalendarService outlookCalendar)
        {
            this.outlookCalendar = outlookCalendar;
        }


        [HttpGet]
        public async Task<IActionResult> GetCalendarEvents()
        {
            var calendarEvents = await outlookCalendar.GetCalendarEvents();
            return Ok(calendarEvents);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCalendarEvent(CalendarEvent evt)
        {
            await outlookCalendar.CreateCalendarEvent(evt);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCalendarEvent(CalendarEvent evt)
        {
            await outlookCalendar.UpdateCalendarEvent(evt);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCalendarEvent(string id)
        {
            await outlookCalendar.DeleteCalendarEvent(id);
            return Ok();
        }
    }
}
