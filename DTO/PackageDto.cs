namespace EcoPowerHub.DTO
{
    public class PackageDto
    {
        public int Id { get; set; }
        public string Details { get; set; }
        public decimal Price { get; set; }
        public byte[] Image { get; set; }
        public int CompanyId { get; set; }
    }
}
