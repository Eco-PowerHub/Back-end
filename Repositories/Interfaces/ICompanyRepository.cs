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
        Task<ResponseDto> GetCompanyProducts(string companyName);
     //   Task<ResponseDto> GetCompanyPackages(string companyName);
        Task<ResponseDto> AddCompany(CompanyDto company);
        Task<ResponseDto> UpdateCompany(int id , CompanyDto company);
        Task<ResponseDto> DeleteCompany(int id);
    }
}
