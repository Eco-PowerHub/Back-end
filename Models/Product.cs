using System.ComponentModel.DataAnnotations.Schema;

namespace EcoPowerHub.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock {  get; set; }
        public int Amount { get; set; }
        public string Image { get; set; }
        public string Price { get; set; }
        public string Model { get; set; }
        public string Efficiency { get; set; }
        public string EstimatedPower { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
    }
}
