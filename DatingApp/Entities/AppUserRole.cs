using Microsoft.AspNetCore.Identity;
using WebAPIDatingAPP.Entities;

namespace DatingApp.Entities
{
    public class AppUserRole:IdentityUserRole<int>
    {
        public AppUsers User { get; set; }
        public AppRole Role { get; set; }

    }
}
