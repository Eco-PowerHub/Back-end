using EcoPowerHub.DTO;
using EcoPowerHub.DTO.PackageDto;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;

namespace EcoPowerHub.Repositories.Interfaces
{
    public interface IPackageRepository: IGenericRepository<Package>
    {
        Task<ResponseDto> AddPackageAsync(PackageDto packageDto);
        Task<ResponseDto> GetPackagesByCompanyName(string companyName);
        Task<ResponseDto> GetAllPackagesAsync();
        Task<ResponseDto> GetPackageById(int id);
          Task<ResponseDto> UpdatePackageAsync(int id, PackageDto packageDto);
        Task<ResponseDto> DeletePackageAsync(int id);
       // Task<ResponseDto> AddPackageToCart(int packageId, int cartId);
       // Task<ResponseDto> GetCurrentUserPackageAsync();

    }
}
