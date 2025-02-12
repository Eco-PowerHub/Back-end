using AutoMapper;
using EcoPowerHub.DTO;
using EcoPowerHub.Models;

namespace EcoPowerHub.AutoMapper
{
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            CreateMap<PackageDto, Package>().ReverseMap();
        }
    }
}
