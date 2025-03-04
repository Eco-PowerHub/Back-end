using EcoPowerHub.DTO;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;

namespace EcoPowerHub.Repositories.Interfaces
{
    public interface IProductRepository: IGenericRepository<Product>
    {
        Task<ResponseDto> GetAllProductsAsync();
        Task<ResponseDto> GetProductById(int id);
        Task<ResponseDto> GetProductByName(string name);
        Task<ResponseDto> GetProductByCategory(string categoryName);
        Task<ResponseDto> GetProductByCompany(string companyName);
        Task<ResponseDto> GetProductsSortedByPrice(string categoryName);
        Task<ResponseDto> AddProductAsync(ProductDto productDto);
        Task<ResponseDto> UpdateProductAsync(int id,ProductDto productDto);
        Task<ResponseDto> DeleteProductAsync(int id);
        Task<ResponseDto> SearchProductAsync(string searchTerm); 
    }
}
