using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoPower.Models
{
    public class ApplicationUser :IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        public string? Address { get; set; }
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public byte[]? ProfilePic { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Property> Properties { get; set; } = new List<Property>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public ICollection<Support> Supports { get; set; } = new List<Support>();
    }
}
