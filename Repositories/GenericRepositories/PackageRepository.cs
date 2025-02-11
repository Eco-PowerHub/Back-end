using AutoMapper;
using EcoPowerHub.Data;
using EcoPowerHub.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoPowerHub.Repositories.GenericRepositories
{
    public class PackageRepository : IGenericRepository<Package>
    {
        private readonly EcoPowerDbContext _context;
        private readonly IMapper _mapper;
        public PackageRepository(EcoPowerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Package> AddAsync(Package package)
        {
            await _context.Packages.AddAsync(package);
            await _context.SaveChangesAsync();
            return package;
        }

        public async Task<Package> DeleteAsync(int id)
        {
            var package= await _context.Packages.FindAsync(id);
            _context.Packages.Remove(package);
            await _context.SaveChangesAsync();
            return package;
            
        }

        public async Task<IEnumerable<Package>> GetAllAsync()
        {
            IEnumerable<Package> packages = await _context.Packages.ToListAsync();
            return packages;
        }

        public async Task<Package> GetByIdAsync(int id)
        {
            var package = await _context.Packages.FindAsync(id);
            return package;
        }

        public async Task<Package> UpdateAsync(Package package)
        {
            _context.Update(package);
            await _context.SaveChangesAsync();
            return package;
        }
    }
}
