using EcoPowerHub.DTO;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;

namespace EcoPowerHub.Repositories.Interfaces
{
    public interface ICategoryRepository :IGenericRepository<Category>
    {
        Task<ResponseDto> AddAsync(Category category);
        Task<ResponseDto> GetAllAsync();
        Task<ResponseDto> GetById(int id);
        Task<ResponseDto> GetByName(string name);
        Task<ResponseDto> UpdateAsync(int id, Category category);
        Task<ResponseDto> DeleteAsync(int id);
    }
}
