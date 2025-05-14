using AutoMapper;
using EcoPowerHub.Data;
using EcoPowerHub.DTO;
using EcoPowerHub.DTO.CartDto;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using EcoPowerHub.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EcoPowerHub.Repositories.Services
{
    public class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
    {
        private readonly EcoPowerDbContext _context;
        private readonly IMapper _mapper;

        #region Constructor
        public CartItemRepository(EcoPowerDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion

        #region Services implementation
        public async Task<ResponseDto> GetAllItems()
        {
            List<CartItem> cartItems = await _context.CartItems.AsNoTracking().Include(p => p.Product).ToListAsync();
            if (!cartItems.Any())
            {
                return new ResponseDto
                {
                    Message = "Items not found!!",
                    IsSucceeded = false,
                    StatusCode = 404,
                    Data = new List<CartItem>()
                };
            }
            var CartList = _mapper.Map<List<CartItemDto>>(cartItems);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                Data = CartList
            };
        }
        public async Task<ResponseDto> AddItem(CartItemDto item)
        {
            var cartExists = await _context.Carts.AnyAsync(c => c.Id == item.CartId);
            if (!cartExists)
            {
                return new ResponseDto
                {
                    Message = "Cart not found!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            var productExists = await _context.Products.AnyAsync(p => p.Id == item.ProductId);
            if (!productExists)
            {
                return new ResponseDto
                {
                    Message = "product you try to add doesn't exist!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            var addedItem = _mapper.Map<CartItem>(item);
            _context.CartItems.Add(addedItem);
            await _context.SaveChangesAsync();
            var dto = _mapper.Map<CartItemDto>(addedItem);
            return new ResponseDto
            {
                Message = "Item added to cart successfully",
                IsSucceeded = true,
                StatusCode = 201,
                Data = dto
            };
        }
        public async Task<ResponseDto> UpdateItem(int id, CartItem cartItem)
        {
            var existingItem = await _context.CartItems.FindAsync(id);
            if (existingItem == null)
            {
                return new ResponseDto
                {
                    Message = "item not found!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            var productExists = await _context.Products.AnyAsync(p => p.Id == cartItem.ProductId);
            if (!productExists)
            {
                return new ResponseDto
                {
                    Message = "Product not found!",
                    IsSucceeded = false,
                    StatusCode = 400
                };
            }
            existingItem.Quantity = cartItem.Quantity;
            existingItem.ProductId = cartItem.ProductId;
            // _context.CartItems.Update(existingItem);
            await _context.SaveChangesAsync();
            var updatedItem = _mapper.Map<CartItemDto>(existingItem);
            return new ResponseDto
            {
                Message = "Item updated successfully",
                IsSucceeded = true,
                StatusCode = 200,
                Data = updatedItem
            };
        }
        public async Task<ResponseDto> DeleteItem(int id)
        {
            var existingItem = await _context.CartItems.FindAsync(id);
            if (existingItem == null)
            {
                return new ResponseDto
                {
                    Message = "Item not found!!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            _context.CartItems.Remove(existingItem);
            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "Item deleted sucessfully",
                IsSucceeded = true,
                StatusCode = 200
            };
        }

        #endregion

    }
}
