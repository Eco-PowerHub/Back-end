using AutoMapper;
using EcoPowerHub.Data;
using EcoPowerHub.DTO;
using EcoPowerHub.DTO.CartDto;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using EcoPowerHub.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EcoPowerHub.Repositories.Services
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        private readonly EcoPowerDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly IHttpContextAccessor _contextAccessor;
        #region Constructor
        public CartRepository(EcoPowerDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager,IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _usermanager = userManager;
            _contextAccessor = httpContextAccessor;
        }

        #endregion

        #region Service Implementation
        public async Task<ResponseDto> GetAllCart()
        {
            List<Cart> carts = await _context.Carts.AsNoTracking().ToListAsync();
            if (carts.Count == 0)
            {
                return new ResponseDto
                {
                    Message = "No carts found",
                    IsSucceeded = false,
                    StatusCode = 404,
                    Data = new List<Cart>()
                };
            }
            var dto = _mapper.Map<List<CartDto>>(carts);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                Data = dto
            };
        }
        public async Task<ResponseDto> AddCart()
        {
            var userId = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return new ResponseDto
                {
                    Message = "User not found!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }

            var newCart = new Cart
            {
                CustomerId = userId
            };

            await _context.Carts.AddAsync(newCart);
            await _context.SaveChangesAsync();

            var cartDto = _mapper.Map<CartDto>(newCart);

            return new ResponseDto
            {
                Message = "New cart added successfully!",
                IsSucceeded = true,
                StatusCode = 201,
                Data = cartDto 
            };
        }

        public async Task<ResponseDto> DeleteCart(int id)
        {
            var existingCart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == id);
            if (existingCart == null)
            {
                return new ResponseDto
                {
                    Message = "Cart not found!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            _context.Carts.Remove(existingCart);
            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "Cart deleted sucessfully",
                IsSucceeded = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> UpdateCart(int id, CartDto cart)
        {
            var existingCart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == id);
            if (existingCart == null)
            {
                return new ResponseDto
                {
                    Message = "Cart not found!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            _mapper.Map(cart, existingCart);
            await _context.SaveChangesAsync();
            var cartDto = _mapper.Map<CartDto>(existingCart);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                Data = cartDto
            };
        } 
        #endregion
    }
}
