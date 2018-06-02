using System.Threading.Tasks;
using CalendarService.Models;

namespace Digit.FocusService.Services
{
    public interface IScheduleService
    {
        Task Schedule(string userId, Event evt);
    }
}
