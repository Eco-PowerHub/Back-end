using EcoPowerHub.Helpers;
using Microsoft.AspNetCore.Identity;

namespace EcoPowerHub.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Roles Role {  get; set; }
        public string? Address { get; set; }
        public DateTime RegisterdAt { get; set; } = DateTime.UtcNow;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
