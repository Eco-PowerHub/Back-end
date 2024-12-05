using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
namespace Application.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Address { get; set; }
        public DateTime RegisterDate { get; set; }
        public string ProfilePic { get; set; }

        //Navigation Properties
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Property> Properties { get; set; } = new List<Property>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public ICollection<Support> Supports { get; set; } = new List<Support>();

    }
}
