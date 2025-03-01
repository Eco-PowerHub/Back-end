using AutoMapper;
using EcoPowerHub.DTO;
using EcoPowerHub.DTO.UserDto;
using EcoPowerHub.Models;

namespace EcoPowerHub.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LoginDto, ApplicationUser>().ReverseMap();
            CreateMap<RegisterDto, ApplicationUser>().ReverseMap();
            CreateMap<PasswordSettingDto, ApplicationUser>().ReverseMap();
            CreateMap<UserDto, ApplicationUser>().ReverseMap();
            CreateMap<PackageDto, Package>().ReverseMap();
<<<<<<< HEAD
            CreateMap<Company ,CompanyDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
=======
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();

>>>>>>> 746411544b0fc3361e5fe302a2d581138ae854b6
        }
    }
}

       

