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
            CreateMap<PackageDto, Package>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Company, CompanyDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products))
                .ForMember(dest => dest.Packages, opt => opt.MapFrom(src => src.Packages))
                .ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<CreateUserSupportDto, UserSupport>();
            CreateMap<UserSupport, GetUserSupportDto>();
            CreateMap<AddResponseDto, UserSupport>();

        }
    }
}

       

