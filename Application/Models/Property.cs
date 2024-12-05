using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Property
    {
        public int Id { get; set; }
        public double SurfaceArea { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }    
        public string ElectricityUsage { get; set; }

        //foreign key to User
        public int UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
