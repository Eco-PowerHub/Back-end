using AutoMapper;
using EcoPowerHub.Data;
using EcoPowerHub.DTO;
using EcoPowerHub.DTO.UserPropertyDto;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using EcoPowerHub.Repositories.Interfaces;
using System.Net;

namespace EcoPowerHub.Repositories.Services
{
    public class PropertyRepository : GenericRepository<PackageOrder>, IPropertyRepository
    {
        private readonly EcoPowerDbContext _context;
        private readonly IMapper _mapper;
        public PropertyRepository(EcoPowerDbContext context,IMapper mapper) :base(context) 
        {
            _context = context;
            _mapper = mapper;
        }
      
        public Task<ResponseDto> AddProperty(PackageOrderDto packageOrderDto)
        {
            var newProperty = _mapper.Map<PackageOrder>(packageOrderDto);

            var dto = _mapper.Map<PackageOrderDto>(newProperty);

            return Task.FromResult(new ResponseDto
            {
                Message = "New property has been added successfully!",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.Created,
                Data = dto
            });
        }

        public Task<ResponseDto> GetRecommendedPackages()
        {
            throw new NotImplementedException();
        }

    }
}
