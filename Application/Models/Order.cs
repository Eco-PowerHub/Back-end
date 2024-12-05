using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal PaidCost { get; set; }
        public decimal SavingCost { get; set; }

        //foreign key to User
        public int UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        //foreign key to Product 
        public int ProductId { get; set; }
        public Product Product { get; set; }
        //foreign key to Payment
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
    }
}
