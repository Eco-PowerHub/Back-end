using AutoMapper;
using EcoPowerHub.DTO.PackageDto;
using EcoPowerHub.Models;

namespace EcoPowerHub.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            CreateMap<PackageDto, Package>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Package, PackageDto>();

        }
    }
}
