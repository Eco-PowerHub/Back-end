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
        public ICategoryRepository Categories { get; }
        public IProductRepository Products { get; }
        public IUserFeedbackRepository UserFeedbacks { get; }
        Task<bool> SaveCompleted();
    }
}
