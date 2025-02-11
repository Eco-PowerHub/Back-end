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
using System.Security.Principal;
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

            if (user.RefreshTokens?.Any() == true)
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
                    StatusCode = (int)HttpStatusCode.NotFound
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
                    StatusCode = (int)HttpStatusCode.NotFound
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
        public async Task<ResponseDto> DeleteProfileAsync(LoginDto account)
        {
            var user = await _userManager.FindByEmailAsync(account.Email);
            if (user is null)
                return new ResponseDto
                {
                    Message = "User not found!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            if (user.RefreshTokens?.Any() == true)
                user.RefreshTokens.Clear();
            var result =  await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return new ResponseDto
                {
                    Message = "Failed to delete account, try agin",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            return new ResponseDto
            {
                Message = "Account deleted successfully ,Your Data Will Be Safely Removed",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK
            };
        }
        public async Task<ResponseDto> updateProfile(UserDto profileDto)
        {
            var user = await _userManager.FindByEmailAsync(profileDto.Email);
            if (user is null)
            {
                return new ResponseDto
                {
                    Message = "User not found!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
            _mapper.Map(profileDto, user);  
            var result =  await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new ResponseDto
                {
                    Message = "Failed to update user, try again",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            var updatedUser = _mapper.Map<UserDto>(user);
            return new ResponseDto
            {
                Message = "User updated successfully",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK,
                Data = updatedUser
            };

        }
        public async Task<ResponseDto> GenerateRefreshTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return new ResponseDto
                {
                    Message = "User not found!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
            if (user.RefreshTokens.Any(t=>t.IsActive))
            {
                return new ResponseDto
                {
                    Message = "Token still active!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                };
            }
            var token = _tokenService.GenerateToken(user);
            var refreshToken = _tokenService.GeneraterefreshToken();
            user.RefreshTokens.Add(refreshToken);
            var updateResult=  await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return new ResponseDto
                {
                    Message = "Failed to update user!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK,
                Data = new
                {
                    IsAuthenticated = true,
                    Token = token,
                    RefreshToken = refreshToken,
                    UserName = user.UserName,
                    Email = user.Email,
                }
            }; 
        }
      
        public async Task<bool> RevokeRefreshTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
               if(user == null) return false;

            var activeToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
            if(activeToken == null) return false;

            activeToken.RevokedOn = DateTime.UtcNow;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return false;
            return true;
        }
        

    }
}
