using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        //foreign key to User
        public int UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
