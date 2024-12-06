using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EcoPower.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal PaidCost { get; set; }
        public decimal SavingCost { get; set; }
        public string ClientId { get; set; }
        [JsonIgnore]
        public ApplicationUser Client { get; set; }
        public int ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
        public int? PaymentId { get; set; }
        public Payment Payment { get; set; }
    }
}
