using System.Text.Json.Serialization;

namespace EcoPower.Models
{
    public class Property
    {
        public int Id { get; set; }
        public double SurfaceArea { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string ElectricityUsage { get; set; }
        public string ClientId { get; set; }
        [JsonIgnore]
        public ApplicationUser Client { get; set; }
    }
}
