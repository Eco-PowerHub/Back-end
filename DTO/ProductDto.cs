using EcoPowerHub.Models;
using System.Text.Json.Serialization;

namespace EcoPowerHub.DTO
{
    public class ProductDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Model { get; set; }
        public string Efficiency { get; set; }
        public string EstimatedPower { get; set; }
        public int CategoryId { get; set; }
         //public CompanyDto? Company { get; set; }
        public int CompanyId { get; set; }

    }
}
