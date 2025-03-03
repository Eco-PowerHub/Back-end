namespace EcoPowerHub.DTO
{
    public class FeedbackDto
    {
        public string Content { get; set; }
        public float Rate { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
