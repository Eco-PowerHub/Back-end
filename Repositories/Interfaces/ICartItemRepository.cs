using EcoPowerHub.DTO.CartDto;
using EcoPowerHub.DTO;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;

namespace EcoPowerHub.Repositories.Interfaces
{
    public interface ICartItemRepository:IGenericRepository<CartItem>
    {
        Task<ResponseDto> GetAllItems();
        //Task<ResponseDto> GetItemById(int id);
        Task<ResponseDto> AddToCart(CartItemDto item);
        Task<ResponseDto> UpdateItem(int id, CartItem cartItem);
        Task<ResponseDto> DeleteItem(int id);
    }
}
