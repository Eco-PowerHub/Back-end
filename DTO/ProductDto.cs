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
        public string Model { get; set; }
        public decimal Efficiency { get; set; }
        public int EstimatedPower { get; set; }
        public CategoryDto Category { get; set; }
        public CompanyDto Company { get; set; }
    }
}
