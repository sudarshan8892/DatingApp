using DatingApp.DTOs;
using DatingApp.Entities;
using WebAPIDatingAPP.Entities;

namespace DatingApp.Interfaces
{
    public interface ILikedRepository
    {
        Task <UserLike> GetUserLike (int SourceUserId, int TragetUserId);    
        Task <AppUsers> GetUserWithLike (int UserId);    
        Task <IEnumerable <LikedDto>> GetUserLike (string Predicate,int UserId);    
    }
}
