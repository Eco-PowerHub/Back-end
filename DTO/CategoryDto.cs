using System.ComponentModel.DataAnnotations;

namespace EcoPowerHub.DTO
{
    public class CategoryDto
    {
        //  public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public List<ProductDto>? products { get; set; }

    }
}
