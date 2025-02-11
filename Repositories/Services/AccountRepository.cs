using AutoMapper;
using EcoPowerHub.Data;
using EcoPowerHub.DTO;
using EcoPowerHub.DTO.UserDto;
using EcoPowerHub.Helpers;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using EcoPowerHub.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Net;
using HttpStatusCode = System.Net.HttpStatusCode;
namespace EcoPowerHub.Repositories.Services
{
    public class AccountRepository : GenericRepository<ApplicationUser>, IAccountRepository
    {
        private readonly EcoPowerDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        public AccountRepository(EcoPowerDbContext context , UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager,IMapper mapper,ITokenService tokenService) :base(context, context.Set<ApplicationUser>())
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }
        public async Task<ResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await _userManager.FindByEmailAsync(registerDto.Email) is not null || await _userManager.FindByNameAsync(registerDto.UserName) is not null)
                return new ResponseDto { Message = "Email or User Name already exists!!" };
            var user = _mapper.Map<ApplicationUser>(registerDto);
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                    errors += $"{error.Description},";
                return new ResponseDto
                {
                    Message = errors,
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
             await _userManager.AddToRoleAsync(user,registerDto.Role.ToString());
            var token = _tokenService.GenerateToken(user);
            return new ResponseDto
            {
                Message = "User has been registerd successfully",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.Created,
                Data = new
                {
                    Token = token,
                    IsAuthenticated = true,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = registerDto.Role
                }
            };
        }
        public async Task<ResponseDto> LoginAsync(LoginDto loginDto)
        {
          var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null ||! await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return new ResponseDto { Message = "User not found!" };
            var token = _tokenService.GenerateToken(user);
            var refreshToken = string.Empty;
            DateTime refreshTokenExpiration;
            if(user.RefreshTokens!.Any(t=>t.IsActive))
            {
                var activeToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                refreshToken = activeToken.Token;
                refreshTokenExpiration = activeToken.ExpiresOn;
            }
            else
            {
                var newRefreshToken = _tokenService.GeneraterefreshToken();
                refreshToken = newRefreshToken.Token;
                refreshTokenExpiration = newRefreshToken.ExpiresOn;
            }
            return new ResponseDto
            {
                Message = "User login successfully",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK,
                Data = new
                {
                    IsAuthenticated = true,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiration = refreshTokenExpiration,
                    UserName = user.UserName,
                    Email = user.Email,
                }
            };   
        }

        public async Task<ResponseDto> Logout(LoginDto logoutDto)
        {
            var user = await _userManager.FindByEmailAsync(logoutDto.Email);
            if (user == null)
                return new ResponseDto { Message = "User not found!" };

            if (user.RefreshTokens != null)
                user.RefreshTokens.Clear();
            await _userManager.UpdateAsync(user);
            return new ResponseDto
            {
                Message = "Logged out successfully.",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        public async Task<ResponseDto> ChangePasswordAsync(PasswordSettingDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
                return new ResponseDto
                {
                    Message = "User not found!",
                    IsSucceeded = false , 
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if(!result.Succeeded)
            {
                return new ResponseDto
                {
                    Message = "Failed to change password , try agin",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            return new ResponseDto
            {
                Message = "Password has changed successfully",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK
            };
        }
        public async Task<ResponseDto> ResetPasswordAsync(PasswordSettingDto dto)
        {

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
                return new ResponseDto
                {
                    Message = "User not found!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);
            if (!result.Succeeded)
            {
                return new ResponseDto
                {
                    Message = "Failed to change password , try agin",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            return new ResponseDto
            {
                Message = "Passwored reset successfully ",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK
            };
            }
        public Task<ResponseDto> DeleteProfileAsync(LoginDto account)
        {
            throw new NotImplementedException();
        }
        public Task<ResponseDto> updateProfile(UserDto profileDto)
        {
            throw new NotImplementedException();
        }
        public Task<ResponseDto> GenerateRefreshTokenAsync(string email)
        {
            throw new NotImplementedException();
        }
      
        public Task<bool> RevokeRefreshTokenAsync(string email)
        {
            throw new NotImplementedException();
        }
        

    }
}
