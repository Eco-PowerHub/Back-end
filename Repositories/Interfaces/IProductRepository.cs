using EcoPowerHub.DTO;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;

namespace EcoPowerHub.Repositories.Interfaces
{
    public interface IProductRepository: IGenericRepository<Product>
    {
        Task<ResponseDto> GetAllAsync();
        Task<ResponseDto> GetById(int id);
        Task<ResponseDto> GetByName(string name);
        Task<ResponseDto> GetByCategory(int categoryId);
        Task<ResponseDto> GetByCompany(int companyId);
        Task<ResponseDto> GetProductsSortedByPrice(int categoryId);
        Task<ResponseDto> AddAsync(ProductDto productDto);
        Task<ResponseDto> UpdateAsync(int id,ProductDto productDto);
        Task<ResponseDto> DeleteAsync(int id);
        Task<ResponseDto> SearchProductAsync(string searchTerm); 
    }
}
