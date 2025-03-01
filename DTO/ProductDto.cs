namespace EcoPowerHub.DTO
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
<<<<<<< HEAD
        public int? CategoryId { get; set; }
=======
        public int CategoryId { get; set; }
>>>>>>> 746411544b0fc3361e5fe302a2d581138ae854b6
        public int CompanyId { get; set; }
    }
}
