using DatingApp.DTOs;
using DatingApp.Helpers;
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
        
        //member

        Task <MemberDTo>GetMemberByNameAsync( string Name );   
        Task<PageList<MemberDTo>> GetMemberAsync( UserParams userParams);   


    }   
}
