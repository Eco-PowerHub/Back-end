using AutoMapper;
using EcoPowerHub.Data;
using EcoPowerHub.DTO;
using EcoPowerHub.DTO.OrderDto;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using EcoPowerHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcoPowerHub.Repositories.Services
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly EcoPowerDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public OrderRepository(EcoPowerDbContext context, IMapper mapper, IEmailService emailService)
            : base(context)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<ResponseDto> Checkout(CreateOrderDto dto)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.Id == dto.CartId && c.CustomerId == dto.UserId);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                return new ResponseDto
                {
                    Message = "Cart is empty or not found",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }

            var order = new Order
            {
                UserId = dto.UserId,
                Price = cart.TotalPrice,
                OrderDate = DateTime.UtcNow,
                OrderHistory = "Pending",
                CompanyId = dto.CompanyId,
                CartId = cart.Id
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // إيميل تأكيد
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user != null)
            {
                var productsDetails = string.Join("<br/>", cart.CartItems.Select(item =>
                    $"- {item.Product.Name} (الكمية: {item.Quantity})"));

                var emailBody = $@"
                                    <h2>تأكيد الطلب - رقم الطلب #{order.Id}</h2>
                                    <p>شكرًا لك على طلبك من EcoPowerHub!</p>
                                    <p><strong>تاريخ الطلب:</strong> {order.OrderDate:yyyy-MM-dd}</p>
                                    <p><strong>تفاصيل المنتجات:</strong><br/>{productsDetails}</p>
                                    <p><strong>السعر الإجمالي للطلب:</strong> {cart.TotalPrice:N2} EGP</p>
                                    <p>سنتواصل معك قريبًا لتأكيد عملية الشحن. شكرًا لثقتك بنا!</p>
                                ";


                Console.WriteLine("Preparing to send email...");
                await _emailService.SendEmailAsync(user.Email, "تأكيد الطلب", emailBody);
                Console.WriteLine("Email sent.");
            }



            await _context.SaveChangesAsync();

            return new ResponseDto
            {
                Message = "Order created successfully and email sent",
                IsSucceeded = true,
                StatusCode = 201,
                Data = new { OrderId = order.Id }
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
    }

}

