using AutoMapper;
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
        private readonly IMapper _mapper;

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _token = tokenService;
            _mapper = mapper;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UsersDTo>> Register(RegisterDto registerDto)
        {


            if (await UserExists(registerDto.Username)) return BadRequest("User Name already exist");

            var user = _mapper.Map<AppUsers>(registerDto);

            user.UserName = registerDto.Username.ToLower();

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UsersDTo
            {
                UserName = user.UserName,
                Token = _token.CreateToken(user),
                KnownAs= user.KnownAs,
                Gender=user.Gender
                


            };
        }
        [HttpPost("login")]
        public async Task<ActionResult<UsersDTo>> Login(LoginDTo loginDTo)
        {
            var user = await _context.Users.Include(p => p.Photos).
                SingleOrDefaultAsync(x => x.UserName == loginDTo.UserName.ToLower());
            if (user == null)
            {
                return Unauthorized("Invalid Users");
            }
       

            return new UsersDTo
            {
                UserName = user.UserName,
                Token = _token.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs=user.KnownAs,
                Gender = user.Gender
            };

        }
        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
