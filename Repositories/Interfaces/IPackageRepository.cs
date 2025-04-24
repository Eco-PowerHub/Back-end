using EcoPowerHub.DTO;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;

namespace EcoPowerHub.Repositories.Interfaces
{
    public interface IPackageRepository: IGenericRepository<BasePackage>
    {
        //Task<ResponseDto> AddPackageAsync(PackageDto packageDto);
        //Task<ResponseDto> GetPackagesByCompanyName(string companyName);
      //  Task<ResponseDto> GetAllPackagesAsync();
      //  Task<ResponseDto> GetPackageById(int id);
      ////  Task<ResponseDto> UpdatePackageAsync(int id, PackageDto packageDto);
      //  Task<ResponseDto> DeletePackageAsync(int id);
    }
}
