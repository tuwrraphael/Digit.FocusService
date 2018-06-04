using CalendarService.Models;
using Digit.FocusService.Services;
using System.Threading.Tasks;
using TravelService.Client;
using TravelService.Models.Directions;

namespace Digit.FocusService.Impl
{
    public class ScheduleService : IScheduleService
    {
        private readonly ITravelServiceClient travelServiceClient;
        private readonly IScheduleStore scheduleStore;

        public ScheduleService(ITravelServiceClient travelServiceClient, IScheduleStore scheduleStore)
        {
            this.travelServiceClient = travelServiceClient;
            this.scheduleStore = scheduleStore;
        }

        private async Task ScheduleNewEvent(string userId, Event evt)
        {
            var scheduledAction = await scheduleStore.Schedule(userId, evt);
            TransitDirections directions = null;
            if (!string.IsNullOrEmpty(evt.Location))
            {
                directions = await travelServiceClient.Directions.Transit.Get(evt.Location, evt.Start, userId);
            }
            if (null != directions)
            {
                await scheduleStore.UpdateDirections(scheduledAction, directions);
            }
        }

        private async Task UpdateScheduledEvent(dynamic scheduledAction, Event evt)
        {

        }

        public async Task Schedule(string userId, Event evt)
        {
            var scheduledAction = await scheduleStore.GetScheduled(userId, evt);
            if (null == scheduledAction)
            {
                await ScheduleNewEvent(userId, evt);
            }
            else
            {
                await UpdateScheduledEvent(scheduledAction);
            }
        }
    }
}
