using EcoPowerHub.Models;

namespace EcoPowerHub.Repositories.Interfaces
{
    public interface ITokenService
    {
         Task<string> GenerateToken(ApplicationUser user);
         RefreshToken GeneraterefreshToken();
    }
}
