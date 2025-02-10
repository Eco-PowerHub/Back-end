using EcoPowerHub.Repositories.Interfaces;

namespace EcoPowerHub.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        public IAccountRepository AccountRepository { get; }
        public ITokenService TokenService { get; }
        Task<int> SaveCompleted();

    }
}
