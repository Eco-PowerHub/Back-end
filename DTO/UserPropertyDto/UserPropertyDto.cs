using EcoPowerHub.Helpers;

namespace EcoPowerHub.DTO.UserPropertyDto
{
    public class UserPropertyDto
    {
        public PropertyType Type { get; set; }
        public string Location { get; set; }
        public decimal SurfaceArea { get; set; }
        public decimal? PackagePrice { get; set; }
        public int? PackageId { get; set; }
        public decimal[] ElectricityUsage { get; set; } = new decimal[6];
        public decimal ElectricityUsageAverage => ElectricityUsage.Length > 0 ? ElectricityUsage.Average() : 0;
        public decimal PricePerYear => ElectricityUsageAverage * 12;

        public float ROIYears => PricePerYear > 0 ? (float)(PackagePrice / PricePerYear) : 0;
        public decimal SavingCost => ROIYears > 0 ? PricePerYear * (30 - (decimal)ROIYears) : 0;

    }
}
