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
<<<<<<< HEAD
>>>>>>> 746411544b0fc3361e5fe302a2d581138ae854b6
=======
        public IUserFeedbackRepository UserFeedbacks { get; }
        public ISupportRepository Supports { get; }
>>>>>>> 7740503d34511a176c63549010bb78c4a74d82cf
        Task<bool> SaveCompleted();
    }
}
