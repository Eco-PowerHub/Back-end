using EcoPowerHub.DTO.CartDto;

namespace EcoPowerHub.DTO.OrderDto
{
    public class OrderDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderHistory { get; set; }

        public string CompanyName { get; set; }
        public string UserEmail { get; set; }

        public List<CartItemDto> CartItems { get; set; }
    }
}
