using EcoPowerHub.DTO.CartDto;

namespace EcoPowerHub.DTO.OrderDto
{
    public class OrderDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; } = "Pending";

        public string CompanyName { get; set; }
        public string UserId { get; set; }

        // public List<CartItemDto> CartItems { get; set; }
    }
}

