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
            List<CartItem> cartItems = await _context.CartItems
                .Include(p => p.Product)
                .AsNoTracking()
                .ToListAsync();

            if (!cartItems.Any())
            {
                return new ResponseDto
                {
                    Message = "Items not found!!",
                    IsSucceeded = false,
                    StatusCode = 404,
                    Data = new List<CartItemDto>()
                };
            }

            var cartList = _mapper.Map<List<CartItemDto>>(cartItems);

            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                Data = cartList
            };
        }

        public async Task<ResponseDto> AddToCart(CartItemDto dto)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == dto.CartId);
            if (cart == null)
                return new ResponseDto { IsSucceeded = false, Message = "Cart not found", StatusCode = 404 };

            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null)
                return new ResponseDto { IsSucceeded = false, Message = "Product not found", StatusCode = 404 };

            var cartItem = new CartItem
            {

                CartId = cart.Id,
                ProductId = product.Id,
                Quantity = dto.Quantity
            };
           
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

     
            var cartItems = await _context.CartItems
                .Where(ci => ci.CartId == cart.Id)
                .Include(ci => ci.Product)
                .ToListAsync();

            decimal totalCartPrice = cartItems.Sum(ci => ci.Product.Price * ci.Quantity);
            cart.TotalPrice = totalCartPrice;
            await _context.SaveChangesAsync();

            int numberOfItems = cartItems.Count;
            int totalQuantity = cartItems.Sum(ci => ci.Quantity);
            //  int totalQuantity = cartItems.Sum(ci => ci.Quantity);

            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 201,
                Message = "Item added to cart successfully",
                Data = new
                {
                    Id  = cartItem.Id,
                    CartId = cart.Id,
                    TotalPrice = totalCartPrice,
                    NumberOfItems = numberOfItems,   
                    totalQuantity = totalQuantity
                }
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
