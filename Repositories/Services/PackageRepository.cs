using AutoMapper;
using EcoPowerHub.Data;
using EcoPowerHub.DTO;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using EcoPowerHub.Repositories.Interfaces;
using EcoPowerHub.UOW;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EcoPowerHub.Repositories.Services
{
    public class PackageRepository : GenericRepository<BasePackage>, IPackageRepository
    {
        private readonly EcoPowerDbContext _context;
        private readonly IMapper _mapper;

        public PackageRepository(EcoPowerDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
            _context = context;
        }
        //public async Task<ResponseDto> GetAllPackagesAsync()
        //{
        //    var packages = await _context.Packages.AsNoTracking().ToListAsync();
        //    if (packages.Count == 0 || !packages.Any())
        //    {
        //        return new ResponseDto
        //        {
        //            Message = "No packages found",
        //            IsSucceeded = false,
        //            StatusCode = (int)HttpStatusCode.NotFound,
        //            Data = new List<PackageDto>()
        //        };
        //    }
        //    var packageDtos = _mapper.Map<IEnumerable<PackageDto>>(packages);
        //    return new ResponseDto
        //    {
        //        Message = "Packages retrieved successfully",
        //        IsSucceeded = true,
        //        StatusCode = 200,
        //        Data = packageDtos
        //    };
        //}
        //public async Task<ResponseDto> GetPackageById(int id)
        //{
        //    if (id <= 0)
        //        return new ResponseDto
        //        {
        //            Message = "Invalid Package id",
        //            IsSucceeded = false,
        //            StatusCode = (int)HttpStatusCode.BadRequest
        //        };

        //    var package = await _context.Packages.FindAsync(id);
        //    if (package is null)
        //    {
        //        return new ResponseDto
        //        {
        //            Message = "Package not found!",
        //            IsSucceeded = false,
        //            StatusCode = (int)HttpStatusCode.NotFound
        //        };
        //    }
        //    var packageDto = _mapper.Map<PackageDto>(package);
        //    return new ResponseDto
        //    {
        //        Message = "Package retrieved successfully",
        //        IsSucceeded = true,
        //        StatusCode = 200,
        //        Data = packageDto
        //    };
        //}
        //public async Task<ResponseDto> GetPackagesByCompanyName(string companyName)
        //{
        //   var company = await _context.Companies.FirstOrDefaultAsync(c=>c.Name == companyName);
        //    if (company is null)
        //        return new ResponseDto
        //        {
        //            Message = "No company found with that name",
        //            IsSucceeded = false,
        //            StatusCode = (int)HttpStatusCode.NotFound
        //        };
        //    var packages = await _context.Packages.Include(c => c.Company)
        //                                          .AsNoTracking()
        //                                          .Where(c => c.CompanyId == company.Id)
        //                                          .ToListAsync();
        //    if (packages.Count == 0)
        //    {
        //        return new ResponseDto
        //        {
        //            Message = "No packages found for the given company",
        //            IsSucceeded = false,
        //            StatusCode = (int)HttpStatusCode.NotFound,
        //            Data = null
        //        };
        //    }
        //    var packagesDto = _mapper.Map<List<PackageDto>>(packages);
        //    return new ResponseDto
        //    {
        //        Message = "Packages retrived  successfully",
        //        IsSucceeded = true,
        //        StatusCode = (int)HttpStatusCode.OK,
        //        Data = packagesDto
        //    };
        //}


        //public async Task<ResponseDto> AddPackageAsync(PackageDto packageDto)
        //{
        //    if (packageDto == null)
        //    {
        //        return new ResponseDto
        //        {
        //            Message = "Invalid Package Data",
        //            IsSucceeded = false,
        //            StatusCode = (int)HttpStatusCode.BadRequest
        //        };
        //    }

        //    // var package = _mapper.Map<Package>(packageDto);
        //    //var package = new BasePackage
        //    //{
        //    //    Id = packageDto.Id,
        //    //    Price = packageDto.Price,
        //    //    Image = packageDto.Image,
        //    //    CompanyId = packageDto.CompanyId,
        //    // //   Details = packageDto.Details
        //    //};
        //    await _context.Packages.AddAsync(package);
        //    await _context.SaveChangesAsync();

        //    var resultDto = _mapper.Map<PackageDto>(package);
        //    return new ResponseDto
        //    {
        //        Message = "Package added successfully!",
        //        IsSucceeded = true,
        //        StatusCode = 201,
        //        Data = resultDto
        //    };
        //}
        //public async Task<ResponseDto> UpdatePackageAsync(int id, PackageDto packageDto)
        //{
        //    if (packageDto is null)
        //    {
        //        return new ResponseDto
        //        {
        //            Message = "Invalid package data!",
        //            IsSucceeded = false,
        //            StatusCode = (int)HttpStatusCode.BadRequest
        //        };
        //    }
        //    var package = await _context.Packages.FindAsync(id);
        //    if (package is null)
        //    {
        //        return new ResponseDto
        //        {
        //            Message = "Package not found!",
        //            IsSucceeded = false,
        //            StatusCode = (int)HttpStatusCode.NotFound
        //        };
        //    }
        //    // _mapper.Map(packageDto, package);
        //    package.Details = packageDto.Details;
        //    package.Price = packageDto.Price;
        //    package.Image = packageDto.Image;
        //    package.CompanyId = packageDto.CompanyId;
        //    _context.Packages.Update(package);
        //    await _context.SaveChangesAsync();

        //    return new ResponseDto
        //    {
        //        Message = "Package updated successfully! ",
        //        IsSucceeded = true,
        //        StatusCode = 200,
        //        Data = packageDto
        //    };
        //}
        //public async Task<ResponseDto> DeletePackageAsync(int id)
        //{
        //    if (id <= 0)
        //        return new ResponseDto
        //        {
        //            Message = "Invalid Package id",
        //            IsSucceeded = false,
        //            StatusCode = (int)HttpStatusCode.BadRequest
        //        };

        //    var package = await _context.Packages.FindAsync(id);
        //    if (package is null)
        //    {
        //        return new ResponseDto
        //        {
        //            Message = "Package not found!",
        //            IsSucceeded = false,
        //            StatusCode = (int)HttpStatusCode.NotFound
        //        };
        //    }
        //    _context.Packages.Remove(package);
        //    await _context.SaveChangesAsync();
        //    return new ResponseDto
        //    {
        //        Message = "Package deleted successfully! ",
        //        IsSucceeded = true,
        //        StatusCode = (int)HttpStatusCode.OK
        //    };
        //}



    }
}