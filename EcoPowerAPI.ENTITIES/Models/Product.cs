using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EcoPower.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public int Stock { get; set; }
        public int PackageId { get; set; }
        [JsonIgnore]
        public Package Package { get; set; }
        public int CategoryId { get; set; }
        [JsonIgnore]
        public Category Category { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
