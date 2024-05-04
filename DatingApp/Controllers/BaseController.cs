using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System.Text;

namespace DatingApp.Controllers
{
    public class BaseController : Controller
    {
        protected bool TryGetToken(out string token)
        {

            token = HttpContext.Session.GetString("UserSessionData");

            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            try
            {
                JObject jsonObject = JObject.Parse(token);
                token = (string)jsonObject["Token"];
                return !string.IsNullOrEmpty(token);
            }
            catch (JsonReaderException)
            {
                // Invalid JSON
                return false;
            }
        }



       
        

    }
}


