using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using EcoPowerHub.Repositories.Interfaces;

namespace EcoPowerHub.UOW
{
    public interface IUnitOfWork : IDisposable
    {

        public IAccountRepository Accounts { get; }
        public ITokenService TokenService { get; }
        public IPackageRepository Packages { get; }
<<<<<<< HEAD
        public ICompanyRepository Company { get; }
=======
        public ICategoryRepository Categories { get; }
        public IProductRepository Products { get; }
>>>>>>> 746411544b0fc3361e5fe302a2d581138ae854b6
        Task<bool> SaveCompleted();
    }
}
