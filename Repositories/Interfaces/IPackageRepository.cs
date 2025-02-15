using EcoPowerHub.DTO;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;

namespace EcoPowerHub.Repositories.Interfaces
{
    public interface IPackageRepository//: IGenericRepository<Package>
    {
        Task<ResponseDto> AddAsync(PackageDto packageDto);
        Task<ResponseDto> GetPackagesByCompanyId(int companyId);
        Task<ResponseDto> GetAllAsync();
        Task<ResponseDto> GetById(int id);
        Task<ResponseDto> UpdateAsync(PackageDto packageDto);
        Task<ResponseDto> DeleteAsync(int id);
    }
}
