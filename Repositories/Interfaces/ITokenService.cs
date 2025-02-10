using EcoPowerHub.Models;

namespace EcoPowerHub.Repositories.Interfaces
{
    public interface ITokenService
    {
       string GenerateToken(ApplicationUser user);
        RefreshToken GeneraterefreshToken();

    }
}
