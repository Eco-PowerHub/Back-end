
using EcoPowerHub.Data;
using Microsoft.EntityFrameworkCore;

namespace EcoPowerHub.Repositories.GenericRepositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly EcoPowerDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(EcoPowerDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
          var models = await _dbSet.AsNoTracking().ToListAsync();
            return models;
        }
        public async Task<T> GetById(int id)
        {
            var model = await _dbSet.FindAsync(id);
            return model;
        }
        public async Task<T> AddAsync(T entity)
        {
           await _dbSet.AddAsync(entity);
            return entity; 
        }

        public async Task<T> UpdateAsync(T entity)
        {
            
            _dbSet.Update(entity); 
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<T> DeleteAsync(int id)
        {
            var model = await _dbSet.FindAsync(id);
            if (model != null)
            {
                _dbSet.Remove(model);
                await _context.SaveChangesAsync();
            }
            return model;
        }
    }
}
