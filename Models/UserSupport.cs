namespace EcoPowerHub.Models
{
    public class UserSupport
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Response { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
