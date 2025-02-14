using System.ComponentModel.DataAnnotations.Schema;

namespace EcoPowerHub.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string Details { get; set; }
        public decimal Price { get; set; }
        public string Image   { get; set; }
        public decimal EnergyInWatt  { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }

    }
}
