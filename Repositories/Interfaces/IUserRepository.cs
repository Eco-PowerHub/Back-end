using EcoPowerHub.DTO;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;

namespace EcoPowerHub.Repositories.Interfaces
{
    public interface IUserRepository :IGenericRepository<ApplicationUser>
    {
        Task<ResponseDto> GetAllUsersAsync();
    }
}
