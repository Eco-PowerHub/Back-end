using EcoPowerHub.DTO;
using EcoPowerHub.DTO.OrderDto;
using EcoPowerHub.DTO.PackageDto;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;

namespace EcoPowerHub.Repositories.Interfaces
{
    public interface IOrderRepository :IGenericRepository<Order>
    {
        Task<ResponseDto> Checkout(int cartId);
        Task<ResponseDto> CheckoutPackage(CheckoutPackageDto dto);
        Task<ResponseDto> GetAllOrders();
        Task<ResponseDto> GetOrderById(int id);
        Task<ResponseDto> DeleteOrder(int id);
        Task<ResponseDto> GetOrdersByCompanyId(int companyId);
        Task<ResponseDto> GetOrdersByCompanyName(string companyName);
        Task<ResponseDto> GetOrdersByUserId(string userId);
    }
}
