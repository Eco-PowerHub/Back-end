using EcoPower.Models;

namespace EcoPower.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        string CreateToken(ApplicationUser user, IList<string> roles);
    }
}
