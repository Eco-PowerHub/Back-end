using AutoMapper;
using EcoPowerHub.DTO.UserDto;
using EcoPowerHub.Models;

namespace EcoPowerHub.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
          CreateMap<LoginDto,ApplicationUser>().ReverseMap();
          CreateMap<RegisterDto,ApplicationUser>().ReverseMap();
          CreateMap<PasswordSettingDto,ApplicationUser>().ReverseMap();
            CreateMap<UserDto, ApplicationUser>().ReverseMap();
        
        }
    }
}
