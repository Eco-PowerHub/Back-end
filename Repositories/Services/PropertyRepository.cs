using AutoMapper;
using EcoPowerHub.Data;
using EcoPowerHub.DTO;
using EcoPowerHub.DTO.PackageDto;
using EcoPowerHub.DTO.UserPropertyDto;
using EcoPowerHub.Helpers;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using EcoPowerHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EcoPowerHub.Repositories.Services
{
    public class PropertyRepository : GenericRepository<UserProperty>, IPropertyRepository
    {
        private readonly EcoPowerDbContext _context;
        private readonly IMapper _mapper;
        public PropertyRepository(EcoPowerDbContext context,IMapper mapper) :base(context) 
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto> AddPropertyAndGetRecommendedPackages(UserPropertyDto UserPropertyDto)
        {
            var addedProperty = _mapper.Map<UserProperty>(UserPropertyDto);

            var packagesLoad = await _context.Packages.AsNoTracking().ToListAsync();

            var recommendedPackages = new List<PackageRecommendDto>();

            foreach (var package in packagesLoad)
            {
                decimal electricityUsageAverage = UserPropertyDto.ElectricityUsageAverage;
                decimal roofArea = UserPropertyDto.SurfaceArea;

               
                decimal usageFactor = GetUsageFactor(UserPropertyDto.Type);
                decimal usageWatts = (electricityUsageAverage / usageFactor) * 1000;

                if (roofArea <= 0 || usageWatts <= 0) continue;

                decimal panelEstimatedPower = package.EnergyInWatt;
                int requiredPanels = CalculateNumberOfPanels(usageWatts, panelEstimatedPower);
                decimal neededInverterPower = CalculateInverterPower(usageWatts);

                int batteryCount = 0;
                if (package.BatteryCapacity.HasValue && package.BatteryCapacity.Value > 0)
                {
                    batteryCount = CalculateBatteryCount(usageWatts, package.BatteryCapacity.Value);
                }

                decimal totalPackagePrice = CalculateTotalPackagePrice(requiredPanels, neededInverterPower, batteryCount, package);


                var recommend = new PackageRecommendDto
                {
                    PackageId = package.Id,
                    PackageName = package.Name,
                    PackagePrice = totalPackagePrice,
                    RequiredPanels = requiredPanels,
                    RequiredBatteries = batteryCount,
                    PanelModel = package.SolarPanel,
                    InverterModel = package.Inverter,
                };
                var roiYears = recommend.ROIYears;
                var savingCost = recommend.SavingCost;
                recommendedPackages.Add(recommend);
            }

            if (!recommendedPackages.Any())
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    StatusCode = 404,
                    Message = "لا يوجد حزمة مناسبة بناءً على بياناتك المدخلة."
                };
            }

            addedProperty.PackageId = recommendedPackages.First().PackageId;
            _context.UserProperties.Add(addedProperty);
            await _context.SaveChangesAsync();

            return new ResponseDto
            {
                Data = recommendedPackages,
                IsSucceeded = true,
                Message = "Recommended packages fetched successfully."
            };
        }

        
        private decimal GetUsageFactor(PropertyType type)
        {
            return type switch
            {
                PropertyType.Residential => 1.5m,
                PropertyType.Commercial => 1.2m,
                PropertyType.Governmental => 1.0m,
               
            };
        }
        private decimal CalculateInverterPower(decimal monthlyUsageInWatts)
        {
            var sunlightHours = 5.5m;
            var dailyUsage = monthlyUsageInWatts / 30;
            var customerPowerConsumption = dailyUsage * sunlightHours;
            var inverterCapacity = customerPowerConsumption * 1.3m;
            return Math.Ceiling(inverterCapacity / 1000) * 1000;
        }

        private int CalculateBatteryCount(decimal monthlyUsageInWatts, decimal batteryCapacity)
        {
            var dailyUsage = monthlyUsageInWatts / 30;
            var totalBatteryRequired = dailyUsage / batteryCapacity;
            return (int)Math.Ceiling(totalBatteryRequired);
        }

        private int CalculateNumberOfPanels(decimal monthlyUsageInWatts, decimal panelCapacityWatts)
        {
            var sunlightHours = 5.5m;
            var dailyPanelOutput = panelCapacityWatts * sunlightHours;
            var dailyUsage = monthlyUsageInWatts / 30;
            return (int)Math.Ceiling(dailyUsage / dailyPanelOutput);
        }

        private decimal CalculateTotalPackagePrice(int requiredPanels, decimal inverterKW, int batteryCount, Package package)
        {
            decimal panelCost = requiredPanels * package.PanelPrice;
            decimal inverterCost = inverterKW * package.InverterPricePerKW;
            decimal batteryCost = batteryCount * package.BatteryPrice;

            return panelCost + inverterCost + batteryCost;
        }
    }
}
