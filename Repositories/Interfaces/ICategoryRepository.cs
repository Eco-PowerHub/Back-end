using EcoPowerHub.DTO;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;

namespace EcoPowerHub.Repositories.Interfaces
{
    public interface ICategoryRepository :IGenericRepository<Category>
    {
        Task<ResponseDto> AddCategoryAsync(CategoryDto categoryDto);
        Task<ResponseDto> GetAllCategories();
        Task<ResponseDto> GetCategoryById(int id);
        Task<ResponseDto> GetCategoryByName(string name);
        Task<ResponseDto> UpdateCategoryAsync(int id, CategoryDto categoryDto);
        Task<ResponseDto> DeleteCategoryAsync(int id);
    }
}
