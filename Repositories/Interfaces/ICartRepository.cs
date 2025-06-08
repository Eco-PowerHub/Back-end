using EcoPowerHub.DTO;
using EcoPowerHub.DTO.CartDto;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;

namespace EcoPowerHub.Repositories.Interfaces
{
    public interface ICartRepository :IGenericRepository<Cart>
    {
        Task<ResponseDto> GetAllCart();
        Task<ResponseDto> GetUserCart(int cartId);
     //   Task<ResponseDto> AddCart();
        Task<ResponseDto> UpdateCart(int id, CartDto cart);
        Task<ResponseDto> DeleteCart(int id);
    }
}
