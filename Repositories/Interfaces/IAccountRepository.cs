using EcoPowerHub.DTO;
using EcoPowerHub.DTO.UserDto;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;

namespace EcoPowerHub.Repositories.Interfaces
{
    public interface IAccountRepository :IGenericRepository<ApplicationUser>
    {
        Task<ResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<ResponseDto> LoginAsync(LoginDto loginDto);
        Task<ResponseDto> Logout(LoginDto logoutDto);
        Task<ResponseDto> updateProfile(UserDto  profileDto);
        Task<ResponseDto> DeleteProfileAsync(LoginDto account);
        Task<ResponseDto> ChangePasswordAsync(PasswordSettingDto dto);
        Task<ResponseDto> ResetPasswordAsync(PasswordSettingDto dto);
        Task<ResponseDto> GetRefreshTokenAsync(string email);
       // Task<bool>  RevokeRefreshTokenAsync(string token);

    }
}
