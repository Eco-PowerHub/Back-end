namespace EcoPowerHub.DTO.PackageDto
{
    public class CheckoutPackageDto
    {
        public string UserId { get; set; }
        public int PackageId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
