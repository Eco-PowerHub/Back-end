using EcoPowerHub.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EcoPowerHub.Models
{
    public  class Package
    {
        public int Id { get; set; }
        public  string Name { get; set; }
        public PackageType PackageType { get; set; }
        public string Image { get; set; }
     //   public decimal Price { get; set; }
        public decimal EnergyInWatt  { get; set; } // inverter power
        public string SolarPanel { get; set; }
        public string Inverter {  get; set; }
        public string? Data { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }

        public decimal? BatteryCapacity { get; set; }
        public decimal? BatteryEfficiency { get; set; }

        public decimal PanelPrice { get; set; }
        public decimal InverterPricePerKW { get; set; }
        public decimal BatteryPrice { get; set; }
        //[JsonIgnore]
        //[ForeignKey("CartId")]
        //public virtual Cart? Cart { get; set; }


    }
}
