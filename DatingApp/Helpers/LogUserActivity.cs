using Microsoft.AspNetCore.Mvc.Filters;
using System.Runtime.InteropServices;
using WebAPIDatingAPP.Extension;
using WebAPIDatingAPP.Interfaces;

namespace DatingApp.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public  async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId= resultContext.HttpContext.User.GetUserId();
            var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IRepository>();
            var user= await repo.GetUserByIdAsync((userId));
            user.LastActive = DateTime.Now;
            await repo.SaveAllAsysc(); 
        }
    }
}
