using AutoMapper;
using EcoPowerHub.Data;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.Interfaces;
using EcoPowerHub.Repositories.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EcoPowerHub.UOW
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly EcoPowerDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        public UnitOfWork
            (
            EcoPowerDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            ITokenService tokenService

       
            ) 
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _tokenService = tokenService;

            Accounts = new AccountRepository(_context, _userManager,_roleManager,_mapper,_tokenService);
            
        }
        public IAccountRepository Accounts {  get; private set; }

        public ITokenService TokenService {  get; private set; }

        public async Task<int> SaveCompleted()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
