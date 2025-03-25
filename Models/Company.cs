namespace EcoPowerHub.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Rate { get; set; }
        public string Location { get; set; }
        public string PhoneNumber   { get; set; }
        public string Image { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<BasePackage> Packages { get; set; } = new List<BasePackage>();
    }
}
