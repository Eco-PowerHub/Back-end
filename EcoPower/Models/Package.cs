using System.Text.Json.Serialization;

namespace EcoPower.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string? Details { get; set; }
        public decimal Price { get; set; }
        
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
