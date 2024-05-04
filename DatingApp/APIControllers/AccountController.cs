using DatingApp.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using WebAPIDatingAPP.DATA;
using WebAPIDatingAPP.Entities;
using WebAPIDatingAPP.Interfaces;

namespace DatingApp.Controllers
{
 
    public class AccountController : BaseApiController
    {
        public readonly DataContext _context;
        private readonly ITokenService _token;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _token = tokenService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UsersDTo>> Register(RegisterDto registerDto)
        {


            if (await UserExists(registerDto.Username)) return BadRequest("User Name already exist");

            using var hmac = new HMACSHA512();
            var users = new AppUsers()
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.AppUsers.Add(users);
            await _context.SaveChangesAsync();
            return new UsersDTo
            {
                UserName = users.UserName,
                Token = _token.CreateToken(users),
                


            };
        }
        [HttpPost("login")]
        public async Task<ActionResult<UsersDTo>> Login(LoginDTo loginDTo)
        {
            var user = await _context.AppUsers.Include(p=>p.Photos).
                SingleOrDefaultAsync(x => x.UserName == loginDTo.UserName.ToLower());
            if (user == null)
            {
                return Unauthorized("Invalid Users");
            }
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTo.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid Password");
                
            }

            return new UsersDTo
            {
                UserName = user.UserName,
                Token = _token.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
            };

        }
        private async Task<bool> UserExists(string username)
        {
            return await _context.AppUsers.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
