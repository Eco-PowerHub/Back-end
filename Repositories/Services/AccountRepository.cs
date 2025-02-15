using AutoMapper;
using EcoPowerHub.Data;
using EcoPowerHub.DTO;
using EcoPowerHub.DTO.UserDto;
using EcoPowerHub.Helpers;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using EcoPowerHub.Repositories.Interfaces;
using EcoPowerHub.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
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
        private readonly ILogger<UnitOfWork> _logger;
  
        public AccountRepository(EcoPowerDbContext context , UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager,IMapper mapper,ITokenService tokenService,ILogger<UnitOfWork> logger) :base(context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _logger = logger;
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
            var refreshToken = "";
            DateTime refreshTokenExpiration;
            if(user.RefreshTokens!.Any(t=>t.IsActive))
            {
                var activeToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                refreshToken = activeToken.Token;
                refreshTokenExpiration = activeToken.ExpiresOn;
                _logger.LogInformation($" Existing Refresh Token: {refreshToken} , Expires on: {refreshTokenExpiration}");
            }
            else
            {
                var newRefreshToken = _tokenService.GeneraterefreshToken();
                refreshToken = newRefreshToken.Token;
                refreshTokenExpiration = newRefreshToken.ExpiresOn;
                user.RefreshTokens.Add(newRefreshToken);
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    _logger.LogError("Failed to save new refresh token.");
                    foreach (var error in updateResult.Errors)
                    {
                        _logger.LogError($"Error Code: {error.Code}, Description: {error.Description}");
                    }
                }
                _logger.LogInformation($"New Refresh Token Generated: {refreshToken}, Expires on: {refreshTokenExpiration}");
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
                _logger.LogError("Failed to update user.");
                foreach (var error in result.Errors)
                {
                    _logger.LogError($"Error Code: {error.Code}, Description: {error.Description}");
                }
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
        public async Task<ResponseDto> GetRefreshTokenAsync(string email)
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
            var activeToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
            if (activeToken is null)
            {
                return new ResponseDto
                {
                    Message = "No refresh tokens found!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
            var token = _tokenService.GenerateToken(user);
            var refreshToken = _tokenService.GeneraterefreshToken();
            user.RefreshTokens!.Add(refreshToken);
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

        //public async Task<IdentityResult> SendOTPAsync(string email)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user == null)
        //        return IdentityResult.Failed(new IdentityError { Description = "User Not Found" });

        //    var otp = _tokenService.GenerateOTP();
        //    user.OTP = otp;
        //    user.OTPExpiry = DateTime.UtcNow.AddMinutes(15);
        //    await _userManager.UpdateAsync(user);

        //    var message = new MailMessage(new[] { user.Email }, "Your OTP", $"Your OTP For Change Your Password In T_ECOM is: {otp}");
        //    _mailingService.SendMail(message);

        //    return IdentityResult.Success;
        //}

        //public Task<IdentityResult> verifyOTPRequest(VerifyOTPRequest request)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<bool> RevokeRefreshTokenAsync(string token)
        //{
        //    var user = await _userManager.Users.SingleOrDefaultAsync(u=>u.RefreshTokens.Any(t=>t.Token==token));

        //    if (user == null)
        //        return false;
        //    var refreshToken = user.RefreshTokens.Single(t=>t.Token == token);
        //    if (!refreshToken.IsActive) return false;
        //    refreshToken.RevokedOn = DateTime.UtcNow;
        //    await _userManager.UpdateAsync(user);
        //    return true;
        //}


    }
}
