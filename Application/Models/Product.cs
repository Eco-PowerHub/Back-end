using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public int Stock { get; set; }

        //foreign key to Package 
        public int PackageId { get; set; }
        public Package Package { get; set; }
        //foreign key to Category
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        //Navigation Property to Order
        public ICollection<Order> Orders { get; set; }= new List<Order>();

    }
}
