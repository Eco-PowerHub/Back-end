using AutoMapper;
using EcoPowerHub.Data;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;

using EcoPowerHub.Repositories.Interfaces;
using EcoPowerHub.Repositories.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EcoPowerHub.UOW
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly EcoPowerDbContext _context;
       
        public IAccountRepository AccountRepository {  get; private set; }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork
            (
            EcoPowerDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            ITokenService tokenService,
            ILogger<UnitOfWork> logger

       
            ) 
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _logger = logger;

            Accounts = new AccountRepository(_context, _userManager,_roleManager,_mapper,_tokenService,_logger);
            
        }
        public IAccountRepository Accounts {  get; private set; }

        public ITokenService TokenService {  get; private set; }

        public IGenericRepository<Package> PackageRepository {  get; private set; }

        public UnitOfWork(EcoPowerDbContext context)
        {
            _context = context;
            PackageRepository = new GenericRepository<Package>(context);
        }
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
