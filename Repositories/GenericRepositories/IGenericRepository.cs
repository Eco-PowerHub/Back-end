namespace EcoPowerHub.Repositories.GenericRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T> GetById(int id);
        public Task<T> AddAsync(T entity);
        public Task<T> UpdateAsync(T entity);
        public Task<T> DeleteAsync(int id);

    }
}
