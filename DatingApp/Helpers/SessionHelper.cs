using DatingApp.DTOs;
using Newtonsoft.Json;
using System.Text;

namespace DatingApp.Helpers
{
    public  class SessionHelper
    {
        public static UsersDTo GetLoginDtoFromSession(HttpContext httpContext)
        {
            byte[] existingSessionDataBytes = httpContext.Session.Get("UserSessionData");

            if (existingSessionDataBytes != null)
            {
                string existingSessionDataJson = Encoding.UTF8.GetString(existingSessionDataBytes);
                return JsonConvert.DeserializeObject<UsersDTo>(existingSessionDataJson);
            }

            return null;
        }
    }
}
