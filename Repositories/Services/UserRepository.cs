using AutoMapper;
using EcoPowerHub.Data;
using EcoPowerHub.DTO;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using EcoPowerHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcoPowerHub.Repositories.Services
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        private readonly EcoPowerDbContext _context;

        public UserRepository(EcoPowerDbContext context) : base(context)
        {
            _context = context;

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
        }
    }
}

