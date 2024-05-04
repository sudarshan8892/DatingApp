using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
