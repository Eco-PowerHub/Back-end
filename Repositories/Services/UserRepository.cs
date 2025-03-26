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
        public async Task<ResponseDto> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            if (users.Count == 0)
                return new ResponseDto
                {
                    Message = "No users found!",
                    IsSucceeded = false 
                };
            var result = users.Select( u => new
            {
                UserID = u.Id,
                UserName = $"{u.FirstName} {u.LastName}",
                PhoneNumber = u.PhoneNumber ?? "N/A",
                RegistrationDate = u.RegisterdAt,
            });
            return new ResponseDto
            {
                Data = result,
                IsSucceeded = true
            };
        }
    }
}

