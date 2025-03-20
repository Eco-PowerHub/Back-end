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
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        //   private readonly ILogger<AccountRepository> _logger;
        private readonly IEmailService _emailService;
        private readonly EmailTemplateService _emailTemplateService;


        public AccountRepository(EcoPowerDbContext context, UserManager<ApplicationUser> userManager
           , IMapper mapper,
            ITokenService tokenService,//ILogger<AccountRepository> logger ,
            IEmailService emailService, EmailTemplateService emailTemplateService) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            //   _logger = logger;
            _emailService = emailService;
            _emailTemplateService = emailTemplateService;
        }
        public async Task<ResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await _userManager.FindByEmailAsync(registerDto.Email) is not null
             || await _userManager.FindByNameAsync(registerDto.UserName) is not null)
                return new ResponseDto { Message = "Email or User Name already exists!!" };

            var otp = GeneratetOtp.GenerateOTP();
            var otpExpiry = DateTime.UtcNow.AddDays(1);

            var user = _mapper.Map<ApplicationUser>(registerDto);
            user.OTP = otp;
            user.OTPExpiry = otpExpiry;
            user.IsConfirmed = false;
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if(!result.Succeeded)
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
         //   var token = _tokenService.GenerateToken(user);
            await _emailService.SendEmailAsync(registerDto.Email, "OTP Email Verfication", $"Hi {registerDto.UserName}" +
                $",Use the code below to verify your Eco PowerHub account. Your OTP is {otp}");
            return new ResponseDto
            {
                Message = "OTP sent to your email. Please verify to complete registration.",
                IsSucceeded = true,
                IsConfirmed = false,
                StatusCode = (int)HttpStatusCode.OK,
                Data = new
                {
                    UserName = user.UserName!,
                }
            };
        }
        
        public async Task<ResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return new ResponseDto
                {
                    Message = "User not found!!",
                    IsSucceeded = false,
                    StatusCode = 400
                };
            }
           
            var token = await _tokenService.GenerateToken(user);
            var refreshToken = string.Empty;
            DateTime refreshTokenExpiration;
            if (user.RefreshTokens!.Any(t => t.IsActive))
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
                Message = " User login successfully ",
                IsSucceeded = true,
                StatusCode = 200,
                Data = new
                {
                    IsAuthenticated = true,
                    token = token,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiration = refreshTokenExpiration,
                    UserName = user.UserName,
                    Email = user.Email
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
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
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
            var result = await _userManager.DeleteAsync(user);
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
            var user = await _userManager.FindByEmailAsync(profileDto.Email!);
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
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    return new ResponseDto
                    {
                        Message = "User not found!",
                        IsSucceeded = false,
                        StatusCode = (int)HttpStatusCode.NotFound
                    };
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
            if (user == null)
            {
                return new ResponseDto
                {
                    Message = "Invalid Email!!",
                    IsSucceeded = false,
                    StatusCode = 400,
                };
            }
            var activeToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
            if (activeToken is not null)
            {
                return new ResponseDto
                {
                    Message = "Token still active",
                    IsSucceeded = false,
                    StatusCode = 400,
                };
            }
            // var token = await _tokenService.GenerateToken(user);
            var refreshToken = _tokenService.GeneraterefreshToken();
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                Data = new
                {
                    IsAuthenticated = true,
                    //       Token = token,
                    RefreshToken = refreshToken,
                    UserName = user.UserName,
                    Email = user.Email,
                }
            };

        }

        public async Task<ResponseDto> SendOTPAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return new ResponseDto
                {
                    Message = "User not found!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };

            var otp = GeneratetOtp.GenerateOTP();
            var otpExpiry = DateTime.UtcNow.AddMinutes(15);
            user.OTP = otp;
            user.OTPExpiry = otpExpiry;
            await _userManager.UpdateAsync(user);


            await _emailService.SendEmailAsync(user.Email!, "OTP Resend", $"Your OTP code is {otp}. It expires in 15 minutes.");
            return new ResponseDto
            {
                Message = "New OTP sent!",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        public async Task<ResponseDto> verifyOTPRequest(VerifyOTPRequest verifyOTP)
        {
            //check for user 
            var user = await _userManager.FindByEmailAsync(verifyOTP.Email);
            if(user is null)
                return new ResponseDto
                {
                    Message = "User not found!",
                    IsSucceeded = false
                };

            if (user.IsConfirmed)
                return new ResponseDto
                {
                    Message = "User already verified!",
                    IsSucceeded = false
                };
            //check otp 
            if (user.OTP != verifyOTP.OTP || user.OTPExpiry < DateTime.UtcNow)
                return new ResponseDto
                {
                    Message = "Invalid or expired OTP!",
                    IsSucceeded = false
                };

            user.IsConfirmed = true;
            user.OTP = null;
            user.OTPExpiry = null;  
            await _userManager.UpdateAsync(user);
          
            var emailBody = _emailTemplateService.RenderWelcomeEmail(
                     user.UserName!,
                     user.Email!,
                     user.Role.ToString()
                  );

            // Send the email
            await _emailService.SendEmailAsync(
                user.Email!,
                "Welcome to EcoPowerHub!",
                emailBody      // Email body (HTML)
            );
            return new ResponseDto
            {
                Message = "User verified successfully!",
                IsSucceeded = true,
                IsConfirmed = true,
                StatusCode = (int)HttpStatusCode.OK
            };
        }
        public async Task<bool> RevokeRefreshTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var activeToken = user.RefreshTokens?.FirstOrDefault(t => t.IsActive);
            if (activeToken != null && activeToken.IsActive) // Check for null before accessing IsActive
            {
                activeToken.RevokedOn = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);
                return true;
            }

            return false;
        }
    }
}

