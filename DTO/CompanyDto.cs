using EcoPowerHub.Models;
using System.ComponentModel.DataAnnotations;

namespace EcoPowerHub.DTO
{
    public class CompanyDto
    {

        [Required]
        public string Name { get; set; }
        public float Rate { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }
        public List<ProductDto>? Products { get; set; } = new List<ProductDto>();
        public List<PackageDto>? Packages { get; set; } = new List<PackageDto>();

    }
}
