using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Support
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Response { get; set; }
        public string Feedback { get; set; }
        public string Details { get; set; }

        //foreign key to User
        public int UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        //foreign key to Engineer 
        public int EngineerId { get; set; }
        public Engineer Engineer { get; set; }
    }
}
