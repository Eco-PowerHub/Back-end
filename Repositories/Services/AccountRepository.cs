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
                    StatusCode = (int)System.Net.HttpStatusCode.BadRequest
                };
            }
             await _userManager.AddToRoleAsync(user,registerDto.Role.ToString());
            var token = _tokenService.GenerateToken(user);
            return new ResponseDto
            {
                Message = "User has been registerd successfully",
                IsSucceeded = true,
                StatusCode = (int)System.Net.HttpStatusCode.Created,
                Data = new
                {
                    Token = token,
                    IsAuthenticated = true,
                    UserName = user.UserName,
                    Role = registerDto.Role
                }
            };
        }
        public Task<ResponseDto> LoginAsync(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> Logout(LoginDto logoutDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> ChangePasswordAsync(PasswordSettingDto dto)
        {
            throw new NotImplementedException();
        }
        public Task<ResponseDto> ResetPasswordAsync(PasswordSettingDto dto)
        {
            throw new NotImplementedException();
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
