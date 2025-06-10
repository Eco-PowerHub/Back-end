using System.Collections.Generic;

namespace EcoPowerHub.DTO.CartDto
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int? CartId { get; set; }
        public int? ProductId { get; set; }
        public int? PackageId { get; set; }
        public string UserId { get; set; }
        public decimal CartPrice { get; set; }
        public ProductDto Product { get; set; }

        //   public decimal? ProductPrice { get; set; }
        // public decimal TotalPrice { get; set; }
    }
}
