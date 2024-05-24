using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;
using WebAPIDatingAPP.Entities;
using WebAPIDatingAPP.Interfaces;

namespace WebAPIDatingAPP.Services
{
    public class TokenServices : ITokenService
    {
       private readonly SymmetricSecurityKey _symmetricSecurityKey; 
        public TokenServices(IConfiguration configuration)
        {
            _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Tokenkey"]));

        }

        public string CreateToken(AppUsers users)
        {
            var claims= new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, users.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, users.UserName)
            };
            var creds = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

            var TokenDiscriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(TokenDiscriptor);
            return tokenHandler.WriteToken(token);
        }
    } 
}
