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
    public class PackageRepository : GenericRepository<Package>, IPackageRepository
    {
        //private readonly EcoPowerDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PackageRepository(EcoPowerDbContext context, IMapper mapper, IUnitOfWork unitOfWork) : base(context)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto> AddAsync(PackageDto packageDto)
        {
            if (packageDto == null)
            {
                return new ResponseDto
                {
                    Message = "Invalid Package Data",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }

            // var package = _mapper.Map<Package>(packageDto);
            var package = new Package
            {
                Id = packageDto.Id,
                Price = packageDto.Price,
                Image = packageDto.Image,
                CompanyId = packageDto.CompanyId,
                Details = packageDto.Details
            };
            await _unitOfWork.PackageRepository.AddAsync(package);


            await _unitOfWork.SaveCompleted();
            var resultDto = _mapper.Map<PackageDto>(package);
            return new ResponseDto
            {
                Message = "Package added successfully!",
                IsSucceeded = true,
                StatusCode = 200,
                Data = resultDto
            };
        }




        public async Task<ResponseDto> GetPackagesByCompanyId(int companyId)
        {
            if (companyId <= 0)
            {
                return new ResponseDto
                {
                    Message = "Invalid company id",
                    IsSucceeded = false,
                    StatusCode = 400,
                    Data = null
                };
            }
            var packages = await _unitOfWork.PackageRepository.GetById(companyId);
                

            if (packages == null )
            {
                return new ResponseDto
                {
                    Message = "No packages found for the given company",
                    IsSucceeded = false,
                    StatusCode = 404,
                    Data = null
                };
            }
            var packageDtos = _mapper.Map<IEnumerable<PackageDto>>(packages);
            return new ResponseDto
            {
                Message = "Packages retrieved successfully",
                IsSucceeded = true,
                StatusCode = 200,
                Data = packageDtos
            };
        }


        public async Task<ResponseDto> DeleteAsync(int id)
        {
            if (id <= 0)
                return new ResponseDto
                {
                    Message = "Invalid Package id",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };

            var package = await _unitOfWork.PackageRepository.GetById(id);
            if (package == null)
            {
                return new ResponseDto
                {
                    Message = "Package not found!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
           await _unitOfWork.PackageRepository.DeleteAsync(id);
            await _unitOfWork.SaveCompleted();

            return new ResponseDto
            {
                Message = "Package deleted successfully! ",
                IsSucceeded = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> GetAllAsync()
        {
            var packages = await _unitOfWork.PackageRepository.GetAllAsync();
            if (packages == null || !packages.Any())
            {
                return new ResponseDto
                {
                    Message = "No packages found",
                    IsSucceeded = true, 
                    StatusCode = 200,
                    Data = new List<PackageDto>()
                };
            }
            var packageDtos = _mapper.Map<IEnumerable<PackageDto>>(packages);
            return new ResponseDto
            {
                Message = "Packages retrieved successfully",
                IsSucceeded = true,
                StatusCode = 200,
                Data = packageDtos
            };
        }

        public async Task<ResponseDto> GetById(int id)
        {
            if (id <= 0)
                return new ResponseDto
                {
                    Message = "Invalid Package id",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };

            var package = await _unitOfWork.PackageRepository.GetById(id);
            if (package == null)
            {
                return new ResponseDto
                {
                    Message = "Package not found!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
            var packageDto = _mapper.Map<PackageDto>(package);
            return new ResponseDto
            {
                Message = "Package retrieved successfully",
                IsSucceeded = true,
                StatusCode = 200,
                Data = packageDto
            };
        }


        public async Task<ResponseDto> UpdateAsync( PackageDto packageDto)
        {
            if (packageDto == null)
            {
                return new ResponseDto
                {
                    Message = "Invalid package data!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            var existingPackage = await _unitOfWork.PackageRepository.GetById(packageDto.Id);
            if (existingPackage == null)
            {
                return new ResponseDto
                {
                    Message = "Package not found!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            _mapper.Map(packageDto, existingPackage);
            await _unitOfWork.PackageRepository.UpdateAsync(existingPackage);
            await _unitOfWork.SaveCompleted();


            return new ResponseDto
            {
                Message = "Package updated successfully! ",
                IsSucceeded = true,
                StatusCode = 200,
                Data = packageDto
            };
        }
    }
}
