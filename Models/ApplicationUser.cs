using EcoPowerHub.Helpers;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public string? OTP { get; set; }
        public DateTime? OTPExpiry { get; set; } = DateTime.UtcNow;
        public bool IsConfirmed { get; set; } = false;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<RefreshToken> RefreshTokens = new List<RefreshToken>();
    }
}
