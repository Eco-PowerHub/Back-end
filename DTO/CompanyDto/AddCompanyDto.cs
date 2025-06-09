using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EcoPowerHub.DTO.CompanyDto
{
    public class AddCompanyDto
    {
        [JsonIgnore]
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        public float Rate { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public string? Image { get; set; }
        public string Email { get; set; }

    }
}
