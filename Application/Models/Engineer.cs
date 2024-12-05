using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Engineer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Specialization { get; set; }

        //Navigation Property to Support
        public ICollection<Support> Supports { get; set; } = new List<Support>();

    }
}
