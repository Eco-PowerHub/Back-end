using EcoPowerHub.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoPowerHub.Models
{
    public abstract  class BasePackage
    {
        public int Id { get; set; }
        public string Name { get; set; }
  //      public PackageType PackageType { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public decimal EnergyInWatt  { get; set; }
        public string SolarPanel { get; set; }
        public string Inverter {  get; set; }
        public string? Data { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }

    }
}
