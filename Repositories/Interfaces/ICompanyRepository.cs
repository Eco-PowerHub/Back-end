using EcoPowerHub.DTO;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;

namespace EcoPowerHub.Repositories.Interfaces
{
    public interface ICompanyRepository :IGenericRepository<Company>
    {
        Task<ResponseDto> GetAllCompany();
        Task<ResponseDto> GetCompanybyId(int companyId);
        Task<ResponseDto> GetCompanybyName(string name);
        Task<ResponseDto> GetCompanyProducts(ProductDto product);
        Task<ResponseDto> GetCompanyPackages(PackageDto packageDto);
        Task<ResponseDto> AddCompany(CompanyDto company);
        Task<ResponseDto> UpdateCompany(int id ,Company company);
        Task<ResponseDto> DeleteCompany(int id);
    }
}
