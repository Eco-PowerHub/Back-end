using EcoPowerHub.Repositories.Interfaces;

namespace EcoPowerHub.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        public IAccountRepository Accounts { get; }
        public ITokenService TokenService { get; }
        Task<int> SaveCompleted();

    }
}
