using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Helpers;
using WebAPIDatingAPP.Entities;

namespace DatingApp.Interfaces
{
    public interface ILikedRepository
    {
        Task <UserLike> GetUserLike (int SourceUserId, int TragetUserId);    
        Task <AppUsers> GetUserWithLike (int UserId);    
        Task <PageList <LikedDto>> GetUserLike (LikesParams likes);    
    }
}
