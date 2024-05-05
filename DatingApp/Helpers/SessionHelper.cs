using DatingApp.DTOs;
using Newtonsoft.Json;
using System.Text;

namespace DatingApp.Helpers
{
    public  class SessionHelper
    {
        public static LoginDTo GetLoginDtoFromSession(HttpContext httpContext)
        {
            byte[] existingSessionDataBytes = httpContext.Session.Get("UserSessionData");

            if (existingSessionDataBytes != null)
            {
                string existingSessionDataJson = Encoding.UTF8.GetString(existingSessionDataBytes);
                return JsonConvert.DeserializeObject<LoginDTo>(existingSessionDataJson);
            }

            return null;
        }
    }
}
