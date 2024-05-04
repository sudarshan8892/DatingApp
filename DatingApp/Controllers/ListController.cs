using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    public class ListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
