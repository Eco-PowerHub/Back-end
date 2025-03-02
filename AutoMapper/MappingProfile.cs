using AutoMapper;
using EcoPowerHub.DTO;
using EcoPowerHub.DTO.UserDto;
using EcoPowerHub.DTO.UserSupportDto;
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
<<<<<<< HEAD

>>>>>>> 746411544b0fc3361e5fe302a2d581138ae854b6
=======
            CreateMap<CreateUserSupportDto, UserSupport>();
            CreateMap<UserSupport, GetUserSupportDto>();
            CreateMap<AddResponseDto, UserSupport>();
>>>>>>> 7740503d34511a176c63549010bb78c4a74d82cf
        }
    }
}

       

