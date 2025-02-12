using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using EcoPowerHub.Repositories.Interfaces;

namespace EcoPowerHub.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        public IAccountRepository AccountRepository { get; }
        public ITokenService TokenService { get; }
        public IGenericRepository<Package> PackageRepository { get; }
        Task<int> SaveCompleted();

    }
}
