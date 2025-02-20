using EcoPowerHub.DTO;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;

namespace EcoPowerHub.Repositories.Interfaces
{
    public interface IPackageRepository: IGenericRepository<Package>
    {
        Task<ResponseDto> AddPackageAsync(PackageDto packageDto);
        Task<ResponseDto> GetPackagesByCompanyId(int companyId);
        Task<ResponseDto> GetAllPackagesAsync();
        Task<ResponseDto> GetPackageById(int id);
        Task<ResponseDto> UpdatePackageAsync(int id, PackageDto packageDto);
        Task<ResponseDto> DeletePackageAsync(int id);
    }
}
