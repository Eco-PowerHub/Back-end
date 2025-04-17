using EcoPowerHub.DTO.PackageDto;
using EcoPowerHub.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EcoPowerHub.DTO.CompanyDto
{
    public class CompanyDto
    {
        [JsonIgnore]
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        public float Rate { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }
        public List<ProductDto>? Products { get; set; }
        //public List<PackageDto>? Packages { get; set; } 
        public List<OffGridPackageDto> OffGridPackages { get; set; }
        public List<OnGridPackageDto> OnGridPackages { get; set; }
    }
}
