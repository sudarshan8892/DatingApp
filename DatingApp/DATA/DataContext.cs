using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebAPIDatingAPP.Entities;

namespace WebAPIDatingAPP.DATA
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions option) : base(option)
        {

        }
       public DbSet<AppUsers> AppUsers { get;set; }

    }
}
