using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string Details { get; set; }
        public decimal Price { get; set; }

        //Navigation Property to Product
        public ICollection<Product> Products { get; set; }= new List<Product>();
    }
}
