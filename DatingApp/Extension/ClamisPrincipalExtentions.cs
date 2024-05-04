
using System.Security.Claims;

namespace WebAPIDatingAPP.Extension
{
    public static class ClamisPrincipalExtentions
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
