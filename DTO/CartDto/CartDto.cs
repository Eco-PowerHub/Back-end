namespace EcoPowerHub.DTO.CartDto
{
    public class CartDto
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        // public ICollection<CartItem>? CartItems { get; set; } = new List<CartItem>();
        public decimal? TotalPrice { get; set; }
    }
}
