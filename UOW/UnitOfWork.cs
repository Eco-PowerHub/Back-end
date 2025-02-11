using EcoPowerHub.Data;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using EcoPowerHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcoPowerHub.UOW
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly EcoPowerDbContext _context;
        public UnitOfWork(EcoPowerDbContext context)
        {
            _context = context;
            
        }
        public IAccountRepository AccountRepository {  get; private set; }

        public ITokenService TokenService {  get; private set; }
        public IGenericRepository<Package> Package {  get; private set; }

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
