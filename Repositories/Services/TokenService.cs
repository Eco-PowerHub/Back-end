using EcoPowerHub.Models;
using EcoPowerHub.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EcoPowerHub.Repositories.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public RefreshToken GeneraterefreshToken()
        {
            byte[] randomNum = new byte[32];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNum);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNum),
                ExpiresOn = DateTime.UtcNow.AddDays(3)
            };
        }

        public string GenerateToken(ApplicationUser user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName!),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(JwtRegisteredClaimNames.Email,user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var Token = new JwtSecurityToken
            (
                issuer: _configuration["JWT:Issuer"],
                audience : _configuration["JWT:Audience"],
                claims : claims,
                signingCredentials : credentials,
                expires : DateTime.UtcNow.AddHours(1)
            );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
