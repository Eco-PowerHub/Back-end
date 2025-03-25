using EcoPowerHub.DTO;
using EcoPowerHub.DTO.UserPropertyDto;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using Org.BouncyCastle.Asn1.Ocsp;

namespace EcoPowerHub.Repositories.Interfaces
{
    public interface IPropertyRepository :IGenericRepository<PackageOrder>
    {
        Task<ResponseDto> AddProperty(PackageOrderDto packageOrderDto);
 //       Task<ResponseDto> DeleteProperty(int id);

        Task<ResponseDto> GetRecommendedPackages();

    }
}
