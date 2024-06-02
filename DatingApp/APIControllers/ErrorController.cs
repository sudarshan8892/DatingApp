using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIDatingAPP.DATA;
using WebAPIDatingAPP.Entities;

namespace DatingApp.Controllers
{
   
    public class ErrorController : BaseApiController
    {
        private readonly DataContext _contxext;

        public ErrorController(DataContext context)
        {
            _contxext = context;
        }
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return ("secret not found");
        }
        [HttpGet("not-Found")]
        public ActionResult<AppUsers> GetNotFound()
        {
            var input = _contxext.Users.Find(-1);

            if (input == null) return NotFound();
            return input;
        }
        [HttpGet("Server-Error")]
        public ActionResult<string> GetSereverError()
        {
            var input = _contxext.Users.Find(-1);
            var returnting = input.ToString();
            return returnting;


        }
        [HttpGet("Bad-Request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("this was a not good request");
        }
    }
}
