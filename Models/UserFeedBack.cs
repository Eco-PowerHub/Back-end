namespace EcoPowerHub.Models
{
    public class UserFeedBack
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public float Rate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //public string UserId { get; set; }
        //public ApplicationUser User { get; set; }
    }
}
