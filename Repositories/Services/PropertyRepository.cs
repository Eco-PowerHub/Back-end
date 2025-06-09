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


        #region Constructor
        public PropertyRepository(EcoPowerDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;

        }

        #endregion
        #region Service Implementation

        public async Task<ResponseDto> AddPropertyAndGetRecommendedPackages(UserPropertyDto dto)
        {

            decimal avgMonthlyCost = dto.ElectricityUsageAverage;
            decimal pricePerKWh = 1.95m;        // motnthlyUsage = 641 KWh -> dialyUsage = 21.37
            decimal monthlyUsageKWh = pricePerKWh > 0
                ? avgMonthlyCost / pricePerKWh
                : 0m;
            decimal adjustedMonthlyUsageKWh = monthlyUsageKWh / pricePerKWh;
            decimal dailyUsageKWh = adjustedMonthlyUsageKWh / 30m;


            var packages = await _context.Packages.AsNoTracking().ToListAsync();
            var recommendedPackages = new List<PackageRecommendDto>();

            foreach (var pkg in packages)
            {
                decimal panelKW = pkg.EnergyInWatt / 1000m; 
                decimal sunlightHours = 5.5m;
                decimal dailyPanelOutputKWh = panelKW * sunlightHours;
                int requiredPanels = ((int)Math.Ceiling(dailyUsageKWh / dailyPanelOutputKWh)) * 2;

                decimal totalPanelArea = requiredPanels * 2.0m;
                if (totalPanelArea > dto.SurfaceArea)
                    continue;

                decimal peakDayUsageKWh = dailyUsageKWh * 0.4m;
                // decimal peakPowerKW = peakDayUsageKWh / sunlightHours;
                decimal inverterPowerW = Math.Ceiling(peakDayUsageKWh * 1.3m * 1000m);

                int batteryCount = 0;
                if (avgMonthlyCost > 3000m && pkg.BatteryCapacity.HasValue && pkg.BatteryCapacity.Value > 0)
                {
                    batteryCount = (int)Math.Ceiling(dailyUsageKWh / (pkg.BatteryCapacity.Value / 1000));
                }

                decimal panelCost = requiredPanels * pkg.PanelPrice;
                decimal inverterCost = (inverterPowerW / 1000m) * pkg.InverterPricePerKW;
                decimal batteryCost = batteryCount * pkg.BatteryPrice;
                decimal totalPrice = panelCost + inverterCost + batteryCost;
 


                recommendedPackages.Add(new PackageRecommendDto
                {
                    PackageId = pkg.Id,
                    PackageName = pkg.Name,
                    RequiredPanels = requiredPanels,
                    RequiredBatteries = batteryCount,
                    PackagePrice = totalPrice,
                    TotalPrice = totalPrice,
                    PanelModel = pkg.SolarPanel,
                    InverterModel = pkg.Inverter,
                    SurfaceArea = dto.SurfaceArea,
                    ElectricityUsage = dto.ElectricityUsage,
                    Image = pkg.Image
                    
                });
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

            var entity = _mapper.Map<UserProperty>(dto);
            entity.PackageId = recommendedPackages.First().PackageId;
            _context.UserProperties.Add(entity);
            await _context.SaveChangesAsync();


            return new ResponseDto
            {
                Data = recommendedPackages;
                // {
                //     recommendedPackages,
                //     TotalYearsSaving = recommendedPackages.Select(p => p.TotalYearsSaving)//.ToList(),

                // },
                IsSucceeded = true,
                Message = "تم جلب الحزم الموصى بها بنجاح."
            };
        } 
        #endregion
    }
    }
   