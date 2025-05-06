using EcoPowerHub.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoPowerHub.Models
{
    public class Order
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string OrderHistory { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int CartId { get; set; }
        public Cart Cart { get; set; }
    }

}
