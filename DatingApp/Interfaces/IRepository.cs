using DatingApp.DTOs;
using WebAPIDatingAPP.Entities;

namespace WebAPIDatingAPP.Interfaces
{
    public interface IRepository
    {
        void Update(AppUsers users);
        Task<bool> SaveAllAsysc();
        Task<IEnumerable<AppUsers>> GetUsersAsysc();
        Task<AppUsers> GetUserByIdAsync( int Id);
        Task<AppUsers> GetUserByNameAsync( string Name );   
        Task<MemberDTo>GetMemberByNameAsync( string Name );   
        Task<IEnumerable<MemberDTo>> GetMemberAsync();   


    }   
}
