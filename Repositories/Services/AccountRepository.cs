using AutoMapper;
using EcoPowerHub.Data;
using EcoPowerHub.DTO;
using EcoPowerHub.DTO.UserDto;
using EcoPowerHub.DTO.UserDto.PasswordSettingDto;
using EcoPowerHub.Helpers;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using EcoPowerHub.Repositories.Interfaces;
using EcoPowerHub.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        #region Constructor
        public AccountRepository(EcoPowerDbContext context, UserManager<ApplicationUser> userManager
         , IMapper mapper,
          ITokenService tokenService,//ILogger<AccountRepository> logger ,
          IEmailService emailService, EmailTemplateService emailTemplateService, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            //   _logger = logger;
            _emailService = emailService;
            _emailTemplateService = emailTemplateService;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Services Implementation
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
            await _userManager.AddToRoleAsync(user, registerDto.Role.ToString());
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
                    otpExpiry = otpExpiry,
                    UserName = user.UserName,
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
            if (user.Role != loginDto.Role)
                return new ResponseDto
                {
                    Message = "Role doesnt found or doesnt match,try again please",
                    IsSucceeded = false,
                    IsConfirmed = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
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
                    Email = user.Email,
                    Role = user.Role,
                    ProfilePicture = user.ProfilePicture ?? "",


                }
            };
        }
        public async Task<ResponseDto> Logout()
        {
           
            var userId = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return new ResponseDto
                {
                    Message = "user not authorized!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                };

            await _tokenService.RevokeRefreshTokenAsync(userId);
            return new ResponseDto
            {
                Message = "Logout successful.",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        public async Task<ResponseDto> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, dto.currentPassword))
                return new ResponseDto
                {
                    Message = "Invalid email or password.",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            var result = await _userManager.ChangePasswordAsync(user, dto.currentPassword, dto.newPassword);
            if (!result.Succeeded)
            {
                return new ResponseDto
                {
                    Message = string.Join(", ", result.Errors.Select(e => e.Description)),
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            return new ResponseDto
            {
                Message = "Password changed successfully",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK
            };
        }
        public async Task<ResponseDto> ForgetPasswordAsync(ForgetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
            {
                return new ResponseDto
                {
                    Message = "If the email is registered, a reset link will be sent.",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            //  ONLY UrlEncode
            var encodedToken = WebUtility.UrlEncode(token);

            var resetLink = $"http://157.175.182.159/resetpassword?email={WebUtility.UrlEncode(dto.Email)}&token={encodedToken}";

            var emailBody = _emailTemplateService.ResetPasswordEmail(dto.Email, resetLink);
            await _emailService.SendEmailAsync(user.Email!, "Reset your password on Eco Power Hub", emailBody);

            return new ResponseDto
            {
                Message = "If the email is registered, a reset link has been sent.",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK
            };
        }


        public async Task<ResponseDto> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
            {
                return new ResponseDto
                {
                    Message = "Invalid email or reset token.",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }

            //  UrlDecode to get the original token
            var decodedToken = WebUtility.UrlDecode(dto.Token);

            var result = await _userManager.ResetPasswordAsync(user, decodedToken, dto.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ResponseDto
                {
                    Message = errors,
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }

            return new ResponseDto
            {
                Message = "Password reset successfully.",
                IsSucceeded = true,
                IsConfirmed = true,
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
            if (user.RefreshTokens.Any() == true)
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
                var errorMassage = string.Join(" , ", result.Errors.Select(e=>e.Description));
                return new ResponseDto
                {
                    Message = $"Update failed: {errorMassage}",
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
            var token = await _tokenService.GenerateToken(user);
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
            if (user is null)
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
        //public async Task<bool> RevokeRefreshTokenAsync(string UserId)
        //{
        //    //var user = await _userManager.Users
        //    //                            .Include(u => u.RefreshTokens)
        //    //                            .FirstOrDefaultAsync(u => u.Email == email);

        //    //if (user == null) return false;

        //    //var activeToken = user.RefreshTokens?.FirstOrDefault(t => t.IsActive);

        //    //if (activeToken is null) return false;

        //    //activeToken.RevokedOn = DateTime.UtcNow;

        //    //var result = await _userManager.UpdateAsync(user);
        //    //if (!result.Succeeded) return false;
        //    ////     await _context.SaveChangesAsync();
        //    //return true;

        //    var user = await _userManager.FindByIdAsync(UserId);
        //    if(user is not null )
        //    {
        //        user.RefreshTokens = null;
        //        await _userManager.UpdateAsync(user);
        //    }
        //    return true;
        //}

        #endregion

    }
}
