using EcoPowerHub.Helpers;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        public TokenService(IConfiguration configuration,UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
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

        public  string GenerateToken(ApplicationUser user)
        {
            var roles = _userManager.GetRolesAsync(user).Result;
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName!),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
             //   new Claim(ClaimTypes.Role,user.Role!),
                new Claim(JwtRegisteredClaimNames.Email,user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
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
