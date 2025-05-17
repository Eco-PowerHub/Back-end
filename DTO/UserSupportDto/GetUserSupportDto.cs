using System.Text.Json.Serialization;

namespace EcoPowerHub.DTO.UserSupportDto
{
    public class GetUserSupportDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Subject { get; set; }
        public string? Response { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
