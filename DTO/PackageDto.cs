using System.Text.Json.Serialization;

namespace EcoPowerHub.DTO
{
    public class PackageDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Details { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int CompanyId { get; set; }
    }
}
