﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EcoPowerHub.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
       // public decimal? Percentage { get; set; }
        public string CustomerId { get; set; }
        [JsonIgnore]
        [ForeignKey("CustomerId")]
        public ApplicationUser Customer { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
       // public ICollection<Package> Packages { get; set; } = new List<Package>();

        public Order Order { get; set; }
    }
}
