using EcoPowerHub.DTO;
using EcoPowerHub.DTO.UserDto;
using EcoPowerHub.DTO.UserDto.PasswordSettingDto;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using Microsoft.AspNetCore.Identity;

namespace EcoPowerHub.Repositories.Interfaces
{
    public interface IAccountRepository :IGenericRepository<ApplicationUser>
    {
        Task<ResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<ResponseDto> LoginAsync(LoginDto loginDto);
        Task<ResponseDto> Logout(LoginDto logoutDto);
        Task<ResponseDto> updateProfile(UserDto  profileDto);
        Task<ResponseDto> DeleteProfileAsync(LoginDto account);
        Task<ResponseDto> ChangePasswordAsync(ChangePasswordDto dto);
        Task<ResponseDto> ForgetPasswordAsync(ForgetPasswordDto dto);
        Task<ResponseDto> ResetPasswordAsync(ResetPasswordDto dto);
        Task<ResponseDto> GetRefreshTokenAsync(string email);
         Task<bool>  RevokeRefreshTokenAsync(string email);
        Task<ResponseDto> SendOTPAsync(string email);
        Task<ResponseDto> verifyOTPRequest(VerifyOTPRequest verifyOTP);


    }
}
