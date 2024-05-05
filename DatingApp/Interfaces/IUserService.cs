using DatingApp.DTOs;

namespace DatingApp.Interfaces
{
    public interface IUserService
    {
        Task<MemberDTo> GetUserByUserNameAsync(string userName,string token);

    }
}
