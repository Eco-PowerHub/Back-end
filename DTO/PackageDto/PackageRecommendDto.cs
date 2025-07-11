﻿namespace EcoPowerHub.DTO.PackageDto
{
    public class PackageRecommendDto
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string Image {  get; set; }
        public decimal SuggestedPrice { get; set; }
        public int RequiredBatteries { get; set; }
        public int RequiredPanels { get; set; }
        public string PanelModel { get; set; }
        public string InverterModel { get; set; }
        public decimal SurfaceArea { get; set; }
        public decimal? PackagePrice { get; set; }
        public decimal[] ElectricityUsage { get; set; } = new decimal[6];
        public decimal TotalPrice { get; set; }


        public decimal ElectricityUsageAverage => ElectricityUsage.Length > 0 ? ElectricityUsage.Average() : 0;
        public decimal PricePerYear => ElectricityUsageAverage * 12;

        public float ROIYears => PricePerYear > 0 ? (float)(PackagePrice / PricePerYear) : 0;
        public decimal SavingCost => ROIYears > 0 ? PricePerYear * (30 - (decimal)ROIYears) : 0;
        public decimal TotalYearsSaving => ROIYears > 0 ? 30 - (decimal)ROIYears : 0;
        public ProductDto Product { get; set; }
    }

    }

