using AutoMapper;
using EcoPowerHub.Data;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;

using EcoPowerHub.Repositories.Interfaces;
using EcoPowerHub.Repositories.Services;
using MailKit.Search;
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

            Accounts = new AccountRepository(_context, _userManager, _mapper, _tokenService, _emailService, _emailTemplateService);
            Packages = new PackageRepository(_context, _mapper);
            Company = new CompanyRepository(_context, _mapper);

            Categories = new CategoryRepository(_context, _mapper);
            Products = new ProductRepository(_context, _mapper);
            UserFeedbacks = new UserFeedbackRepository(_context);
            Supports = new SupportRepository(_context, _mapper);
            Properties = new PropertyRepository(_context, _mapper);
            Users = new UserRepository(_context);
            Carts = new CartRepository(_context, _mapper,_userManager);
            CartItems = new CartItemRepository(_context, _mapper);
            Orders = new OrderRepository(_context, _mapper, _emailService);
        }
        public IAccountRepository Accounts { get; private set; }
        public IUserRepository Users { get; private set; }

        public ITokenService TokenService { get; private set; }

        public IPackageRepository Packages {  get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IProductRepository Products { get; private set; }

        public ICategoryRepository Categories {  get; private set; }
        public IUserFeedbackRepository UserFeedbacks { get; private set; }
        public ISupportRepository Supports { get; private set; }
        public IPropertyRepository Properties { get; private set; }

        public ICartRepository Carts {  get; private set; }

        public ICartItemRepository CartItems {  get; private set; }
        public IOrderRepository Orders { get; private set; }
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