using AutoMapper;
using EcoPowerHub.Data;
using EcoPowerHub.DTO;
using EcoPowerHub.DTO.OrderDto;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using EcoPowerHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace EcoPowerHub.Repositories.Services
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly EcoPowerDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpcontextAccessor;
        private readonly EmailTemplateService _emailTemplateService;


        #region Constructor
        public OrderRepository(EcoPowerDbContext context, IMapper mapper, IEmailService emailService,IHttpContextAccessor httpContextAccessor,EmailTemplateService emailTemplateService) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
            _httpcontextAccessor = httpContextAccessor;
            _emailTemplateService = emailTemplateService;
        }
        #endregion

        #region Service Implementation

        public async Task<ResponseDto> Checkout(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new ResponseDto
                {
                    Message = "User ID is required",
                    IsSucceeded = false,
                    StatusCode = 400
                };
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                  .ThenInclude(p=>p.Package)
                .Include(c => c.CartItems)
                   .ThenInclude(ci => ci.Package)
                .FirstOrDefaultAsync(c => c.CustomerId == userId);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                return new ResponseDto
                {
                    Message = "Cart is empty or not found",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }

            foreach (var item in cart.CartItems)
            {
                if (item.Product == null && item.Package == null)
                {
                    return new ResponseDto
                    {
                        Message = "Invalid cart item: neither product nor package selected.",
                        IsSucceeded = false,
                        StatusCode = 400
                    };
                }
                var product = item.Product;
                if (product != null)
                {
                    if (product.Stock < item.Quantity)
                    {
                        return new ResponseDto
                        {
                            Message = $"Insufficient stock for product: {product.Name}",
                            IsSucceeded = false,
                            StatusCode = 400
                        };
                    }

                    product.Stock -= item.Quantity;
                    product.Amount -= item.Quantity;
                }
             
            }
            

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Price = cart.TotalPrice,
                OrderStatus = "Confirmed",
                CartId = cart.Id,
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var customer = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var emailBody = _emailTemplateService.OrderConfirmationEmail(order);
            await _emailService.SendEmailAsync(customer.Email!, "Order Confirmation Email", emailBody);

            return new ResponseDto
            {
                Message = "Order created successfully and email sent",
                IsSucceeded = true,
                StatusCode = 201,
                Data = new
                {
                    OrderId = order.Id,
                    Price = cart.TotalPrice,
                }
            };
        }


        public async Task<ResponseDto> GetOrderById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Company)
                .Include(o => o.User)
                .Include(o => o.Cart)
                    .ThenInclude(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return new ResponseDto
                {
                    Message = "Order not found",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }

            var orderDto = _mapper.Map<OrderDto>(order);

            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                Data = orderDto
            };
        }


        public async Task<ResponseDto> DeleteOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Cart)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return new ResponseDto
                {
                    Message = "Order not found",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return new ResponseDto
            {
                Message = "Order deleted successfully",
                IsSucceeded = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> GetOrdersByCompanyId(int companyId)
        {
            var orders = await _context.Orders
                .Where(o => o.CompanyId == companyId)
                .Include(o => o.Company)
                .Include(o => o.User)
                .Include(o => o.Cart)
                    .ThenInclude(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                .ToListAsync();

            if (!orders.Any())
            {
                return new ResponseDto
                {
                    Message = "No orders found for this company",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }

            var orderDtos = _mapper.Map<List<OrderDto>>(orders);

            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                Data = orderDtos
            };
        }
        public async Task<ResponseDto> GetOrdersByCompanyName(string companyName)
        {
            var orders = await _context.Orders
                .Include(o => o.Company)
                .Include(o => o.User)
                .Include(o => o.Cart)
                    .ThenInclude(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                .Where(o => o.Company.Name == companyName)
                .ToListAsync();

            if (!orders.Any())
            {
                return new ResponseDto
                {
                    Message = $"No orders found for company '{companyName}'",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }

            var orderDtos = _mapper.Map<List<OrderDto>>(orders);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                Data = orderDtos
            };
        }

        public async Task<ResponseDto> GetOrdersByUserId(string userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Company)
                .Include(o => o.User)
                .Include(o => o.Cart)
                    .ThenInclude(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                .ToListAsync();

            if (!orders.Any())
            {
                return new ResponseDto
                {
                    Message = "No orders found for this user",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }

            var orderDtos = _mapper.Map<List<OrderDto>>(orders);

            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                Data = orderDtos
            };
        }

        public async Task<ResponseDto> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.Company)
                .Include(o => o.User)
                .Include(o => o.Cart)
                    .ThenInclude(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                .ToListAsync();

            if (!orders.Any())
            {
                return new ResponseDto
                {
                    Message = "No orders found",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }

            var orderDtos = _mapper.Map<List<OrderDto>>(orders);

            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                Data = orderDtos
            };
        }
        #endregion
    }
    }
