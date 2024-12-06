using System.Text.Json.Serialization;

namespace EcoPower.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ClientId { get; set; }
        [JsonIgnore]
        public ApplicationUser Client { get; set; }
    }
}
