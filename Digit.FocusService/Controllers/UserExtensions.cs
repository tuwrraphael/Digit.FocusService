using System.Linq;
using System.Security.Claims;

namespace Digit.FocusService.Controllers
{
    public static class UserExtensions
    {
        public static string GetId(this ClaimsPrincipal p)
        {
            return p.Claims.Where(v => v.Type == "sub").Single().Value;
        }
    }
}
