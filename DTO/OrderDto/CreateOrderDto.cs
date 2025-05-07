namespace EcoPowerHub.DTO.OrderDto
{
    public class CreateOrderDto
    {
        public int CartId { get; set; }
        public int CompanyId { get; set; }
        public string UserId { get; set; }
    }
}
