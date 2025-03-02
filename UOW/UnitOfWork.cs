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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UnitOfWork> _logger;
        //    ILogger<AccountRepository> _accountLogger;
        private readonly IEmailService _emailService;
        private readonly EmailTemplateService _emailTemplateService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        // private  ISession Session => _httpContextAccessor.HttpContext?.Session;



        public UnitOfWork
            (
            EcoPowerDbContext context,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            ITokenService tokenService,
        //    ILogger<UnitOfWork> logger,
            IEmailService emailService,
            EmailTemplateService emailTemplateService,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            //            _logger = logger;
            _emailService = emailService;
            _emailTemplateService = emailTemplateService;
            _httpContextAccessor = httpContextAccessor;

            Accounts = new AccountRepository(_context, _userManager, _mapper, _httpContextAccessor, _tokenService, _emailService, _emailTemplateService);
            Packages = new PackageRepository(_context, _mapper);
<<<<<<< HEAD
            Company = new CompanyRepository(_context, _mapper);
=======
            Categories = new CategoryRepository(_context, _mapper);
            Products = new ProductRepository(_context, _mapper);
<<<<<<< HEAD
>>>>>>> 746411544b0fc3361e5fe302a2d581138ae854b6
=======
            UserFeedbacks = new UserFeedbackRepository(_context);
            Supports = new SupportRepository(_context, _mapper);
>>>>>>> 7740503d34511a176c63549010bb78c4a74d82cf
        }
        public IAccountRepository Accounts { get; private set; }

        public ITokenService TokenService { get; private set; }

<<<<<<< HEAD
        public IPackageRepository Packages {  get; private set; }
        public ICompanyRepository Company { get; private set; }
=======
        public IPackageRepository Packages { get; private set; }
        public IProductRepository Products { get; private set; }

        public ICategoryRepository Categories {  get; private set; }
<<<<<<< HEAD
>>>>>>> 746411544b0fc3361e5fe302a2d581138ae854b6

=======
        public IUserFeedbackRepository UserFeedbacks { get; private set; }
        public ISupportRepository Supports { get; private set; }
>>>>>>> 7740503d34511a176c63549010bb78c4a74d82cf
        public async Task<bool> SaveCompleted()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}