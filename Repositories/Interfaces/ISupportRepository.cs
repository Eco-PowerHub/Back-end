using EcoPowerHub.DTO;
using EcoPowerHub.DTO.UserSupportDto;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;

namespace EcoPowerHub.Repositories.Interfaces
{
    public interface ISupportRepository: IGenericRepository<UserSupport>
    {
        Task<ResponseDto> AddSupportAsync(CreateUserSupportDto supportDto);
        Task<ResponseDto> GetAllSupportsAsync();
        Task<ResponseDto> GetSupportByIdAsync(int id);
        Task<ResponseDto> AddResponseAsync(int id, AddResponseDto responseDto);
        Task<ResponseDto> DeleteSupportAsync(int id);
    }
}
