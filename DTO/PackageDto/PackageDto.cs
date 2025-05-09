using EcoPowerHub.Helpers;

namespace EcoPowerHub.DTO.PackageDto
{
    public class PackageDto
    {
        public string Name { get; set; }
        public PackageType PackageType { get; set; }
        public string Image { get; set; }
        public decimal EnergyInWatt { get; set; }
        public string? Data { get; set; }
        public string SolarPanel { get; set; }
        public string Inverter { get; set; }
        public decimal? BatteryCapacity { get; set; }
        public decimal? BatteryEfficiency { get; set; }
        public decimal PanelPrice { get; set; }
        public decimal InverterPricePerKW { get; set; }
        public decimal BatteryPrice { get; set; }
        public int CompanyId { get; set; }
   
    }
}
