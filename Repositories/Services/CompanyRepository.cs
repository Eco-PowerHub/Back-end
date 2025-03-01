using AutoMapper;
using EcoPowerHub.Data;
using EcoPowerHub.DTO;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using EcoPowerHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Net;

namespace EcoPowerHub.Repositories.Services
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        private readonly EcoPowerDbContext _context;
        private readonly IMapper _mapper;
        public CompanyRepository(EcoPowerDbContext context , IMapper mapper) :base(context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResponseDto> GetAllCompany()
        {
            List<Company> companies = await _context.Companies.AsNoTracking().ToListAsync();
            if (companies.Count == 0)
            {
                return new ResponseDto
                {
                    Message = "No companies found!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Data = new List<CompanyDto>()
                };
            }
            var companys = _mapper.Map<List<CompanyDto>>(companies);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK,
                Data = companys
            };
        }

        public async Task<ResponseDto> GetCompanybyId(int companyId)
        {
            var company = await _context.Companies.FindAsync(companyId);
            if (company is null)
            {
                return new ResponseDto
                {
                    Message = "Invalid company ID!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                };
            }
            var retrivedCompany = _mapper.Map<CompanyDto>(company);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK,
                Data = retrivedCompany
            };
        }

        public async Task<ResponseDto> GetCompanybyName(string name)
        {
            var company = await _context.Companies.AsNoTracking().FirstOrDefaultAsync(c => c.Name == name);
            if (company is null)
            {
                return new ResponseDto
                {
                    Message = "Invalid company name!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                };
            }
            var retrivedCompany = _mapper.Map<CompanyDto>(company);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK,
                Data = retrivedCompany
            };
        }
        public async Task<ResponseDto> AddCompany(CompanyDto company)
        {
          bool newCompany = await _context.Companies.AnyAsync(c=>c.Name == company.Name);
            if(newCompany)
            {
                return new ResponseDto
                {
                    Message = "Company name already exists, please enter another name!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                };
            }
            var companyDto = _mapper.Map<Company>(company);
            await _context.Companies.AddAsync(companyDto);
            await _context.SaveChangesAsync();
            var addedCompany = _mapper.Map<CompanyDto>(companyDto);
            return new ResponseDto
            {
               Message = "Company added successfully",
               IsSucceeded = true,
               StatusCode = (int)HttpStatusCode.Created,
               Data = addedCompany
            };
        }

        public async Task<ResponseDto> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company is null)
            {
                return new ResponseDto
                {
                    Message = "Invalid company ID!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
             _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "Company deleted successfully",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK
            };
        }
       
        public async Task<ResponseDto> GetCompanyPackages(PackageDto packageDto)
        {
            var company = await _context.Companies.Include(p=>p.Packages)
                                                    .Where(c=>c.Id == packageDto.CompanyId)
                                                    .ToListAsync();
            if (company.Count == 0)
            {
                return new ResponseDto
                {
                    Message = "No packages found for this company!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Data = null
                };
            }
            var packages = company.SelectMany(c=>c.Packages)
                .Where(p=>p.Id == packageDto.Id)
                .ToList();
            if(packages.Count == 0)
            {
                return new ResponseDto
                {
                    Message = "No matched packages found!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Data = null
                };
            }
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK,
                Data = packages
            };
        }

        public async Task<ResponseDto> GetCompanyProducts(ProductDto product)
        {
            var companies = await _context.Companies.Include(p => p.Products)
                                                    .Where(c => c.Id == product.CompanyId)
                                                    .ToListAsync();
            if (companies.Count == 0)
            {
                return new ResponseDto
                {
                    Message = "No products found for this company!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Data = null
                };
            }
            var selesctedProducts = companies.SelectMany(p=>p.Products)
                                             .Where(p=>p.Id == product.Id)
                                             .ToList();
            if(selesctedProducts.Count == 0)
            {
                return new ResponseDto
                {
                    Message = "No matched products found!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Data = null
                };
            }
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK,
                Data = selesctedProducts
            };
        }

        public async Task<ResponseDto> UpdateCompany(int id, Company company)
        {
            if (company.Id != id)
            {
                return new ResponseDto
                {
                    Message = "Company ID in URL and body do not match!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            var updatedCompany = await _context.Companies.FindAsync(id);
            if (updatedCompany is null)
            {
                return new ResponseDto
                {
                    Message = "Company not found!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound 
                };
            }
            if (await _context.Companies.AnyAsync(c => c.Name == company.Name && c.Id != id))
            {
                return new ResponseDto
                {
                    Message = "Another company with this name already exists!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            updatedCompany.Name = company.Name;
            updatedCompany.Rate = company.Rate;
            updatedCompany.Location = company.Location;
            updatedCompany.PhoneNumber = company.PhoneNumber;
            await _context.SaveChangesAsync();
            var updatedCompanyDto = _mapper.Map<CompanyDto>(updatedCompany);
            return new ResponseDto
            {
                Message = "Company updated successfully",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK,
                Data = updatedCompanyDto
            };
        }

    }
    
}
