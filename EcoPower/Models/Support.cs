using System.Text.Json.Serialization;

namespace EcoPower.Models
{
    public class Support
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Response { get; set; } = "In Progress";
        public string? Feedback { get; set; }
        public string Status { get; set; } = "Open";
        public string ClientId { get; set; }
        public ApplicationUser Client { get; set; }
        public int EngineerId { get; set; }
        public Engineer Engineer { get; set; }
    }
}
