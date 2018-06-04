using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Digit.FocusService.Controllers
{
    [Route("api/callback")]
    public class CallbackController : ControllerBase
    {
        [HttpPost("refreshDirections")]
        public async Task<IActionResult> RefreshDirections()
        {
            return Ok();
        }
    }
}
