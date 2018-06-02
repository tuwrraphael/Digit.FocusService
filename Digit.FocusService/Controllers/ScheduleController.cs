using CalendarService.Models;
using Digit.FocusService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Digit.FocusService.Controllers
{
    [Route("api")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            this.scheduleService = scheduleService;
        }

        [Authorize("User")]
        [HttpPost("me/event")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> ScheduleEvent(Event evt)
        {
            return await ScheduleEvent(User.GetId(), evt);
        }

        [Authorize("Service")]
        [HttpPost("{userId}/event")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> ScheduleEvent(string userId, Event evt)
        {
            await scheduleService.Schedule(userId, evt);
            return Ok();
        }
    }
}
