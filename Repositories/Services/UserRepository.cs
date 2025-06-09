using AutoMapper;
using EcoPowerHub.Data;
using EcoPowerHub.DTO;
using EcoPowerHub.DTO.UserDto;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using EcoPowerHub.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EcoPowerHub.Repositories.Services
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        private readonly EcoPowerDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(EcoPowerDbContext context ,IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager ) : base(context)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;

        }
        private string GetSafeString(object value)
        {
            return value == DBNull.Value || value == null ? string.Empty : value.ToString();
        }

        //public async Task<ResponseDto> GetAllUsersAsync()
        //{
        //    var users = await _context.Users.ToListAsync();
        //    if (users.Count == 0)
        //        return new ResponseDto
        //        {
        //            Message = "No users found!",
        //            IsSucceeded = false
        //        };
        //    var result = users.Select(u => new
        //    {
        //        UserID = u.Id,
        //        UserName = $"{GetSafeString(u.FirstName)} {GetSafeString(u.LastName)}".Trim(),
        //        PhoneNumber = u.PhoneNumber ?? "N/A",
        //        RegistrationDate = u.RegisterdAt,
        //    });
        //    if (users.Count == 0)
        //    {
        //        return new ResponseDto
        //        {
        //            Message = "No users found!",
        //            IsSucceeded = false
        //        };
        //    }
        //    return new ResponseDto
        //    {
        //        Data = result,
        //        IsSucceeded = true
        //    };
        //}
        #region Service Implementation
        public async Task<ResponseDto> GetAllUsersAsync()
        {
            try
            {

                var users = await _context.Users
                    .Select(u => new
                    {
                        UserId = u.Id,
                        FirstName = u.FirstName == null ? string.Empty : u.FirstName,
                        LastName = u.LastName == null ? string.Empty : u.LastName,
                        PhoneNumber = u.PhoneNumber == null ? "N/A" : u.PhoneNumber,
                        RegisterdAt = u.RegisterdAt
                    })
                    .ToListAsync();

                if (users.Count == 0)
                    return new ResponseDto
                    {
                        Message = "No users found!",
                        IsSucceeded = false
                    };

                var result = users.Select(u => new
                {
                    UserID = u.UserId,
                    UserName = $"{u.FirstName} {u.LastName}".Trim(),
                    PhoneNumber = u.PhoneNumber,
                    RegistrationDate = u.RegisterdAt,
                });

                return new ResponseDto
                {
                    Data = result,
                    IsSucceeded = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    Message = $"Error retrieving users: {ex.Message}",
                    IsSucceeded = false
                };
            } 
            #endregion
        }

        public async Task<ResponseDto> GetCurrentUserAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return new ResponseDto
                {
                    Message = "Unauthorized",
                    IsSucceeded = false,
                    StatusCode = 401
                };
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ResponseDto
                {
                    Message = "User not found",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            var roles = await _userManager.GetRolesAsync(user);
            //var userDto = new UserDataDto
            //{
            //    UserId = user.Id, 
            //    Username = user.UserName!,    
            //    Email = user.Email!,
            //    ProfilePicture = user.ProfilePicture ?? "",
            //    Role = user.Role,


            //};
            //var cartId = user.Carts.FirstOrDefault()?.Id;
            var cart = await _context.Carts
                            .AsNoTracking()
                            .FirstOrDefaultAsync(c => c.CustomerId == user.Id);

            int cartId = cart?.Id ?? 0;
            return new ResponseDto
            {
                Message = "User data retrieved successfully",
                IsSucceeded = true,
                StatusCode = 200,
                Data = new 
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = user.Role,
                    ProfilePicture = user.ProfilePicture ?? "",
                    //   CartId = user.Carts!.First().Id
                    CartId = cartId
                }
            };
        }
    }
}

