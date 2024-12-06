namespace EcoPower.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } 
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public string ClientId { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
