using WebAPIDatingAPP.Entities;

namespace WebAPIDatingAPP.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUsers users);
    }
}
