using AutoMapper;
using EcoPowerHub.DTO;
using EcoPowerHub.DTO.CartDto;
using EcoPowerHub.DTO.CompanyDto;
using EcoPowerHub.DTO.OrderDto;
using EcoPowerHub.DTO.PackageDto;
using EcoPowerHub.DTO.UserDto;
using EcoPowerHub.DTO.UserDto.PasswordSettingDto;
using EcoPowerHub.DTO.UserPropertyDto;
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
            CreateMap<ResetPasswordDto, ApplicationUser>().ReverseMap();
            CreateMap<ChangePasswordDto, ApplicationUser>().ReverseMap();
            CreateMap<UserDto, ApplicationUser>().ReverseMap();
            //CreateMap<PackageDto, BasePackage>().ReverseMap()
            //    .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Company, CompanyDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products))
                .ReverseMap();
            CreateMap<AddCompanyDto, Company>().ReverseMap();
            //    .ForMember(dest => dest.Packages, opt => opt.MapFrom(src => src.Packages))
            //    .ReverseMap();
            //   CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<CreateUserSupportDto, UserSupport>();
            CreateMap<UserSupport, GetUserSupportDto>();
            CreateMap<AddResponseDto, UserSupport>();
            CreateMap<UserPropertyDto, UserProperty>().ReverseMap();
            
            CreateMap<Cart,CartDto>().ReverseMap();
            CreateMap<CartItem,CartItemDto>().ReverseMap();

            CreateMap<Package,PackageDto>().ReverseMap();

            CreateMap<Order, OrderDto>()
           .ForMember(d => d.CompanyName, o => o.MapFrom(s => s.Company.Name))
           .ForMember(d => d.UserId, o => o.MapFrom(s => s.UserId));


        }
    }
}

       

