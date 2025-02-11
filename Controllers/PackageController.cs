using AutoMapper;
using EcoPowerHub.DTO.PackageDto;
using EcoPowerHub.Models;
using EcoPowerHub.UOW;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EcoPowerHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PackageController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/<PackageController>
        [HttpGet]
        public async Task<IActionResult> ViewAllPackages()
        {
            var packages = await _unitOfWork.Package.GetAllAsync();
            return Ok(packages);
        }

        // GET api/<PackageController>/5
        [HttpGet("{id}", Name ="GetPackageById")]
        public async Task<IActionResult> GetPackageById(int id)
        {
            var package = await _unitOfWork.Package.GetByIdAsync(id);
            if (package == null)
            {
                return NotFound();
            }
            return Ok(package);
        }

        // POST api/<PackageController>
        [HttpPost]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> CreatePackage([FromBody] PackageDto packageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var package = _mapper.Map<Package>(packageDto);

            await _unitOfWork.Package.AddAsync(package);
            await _unitOfWork.SaveCompleted();

            return CreatedAtAction(nameof(GetPackageById), new { id = package.Id }, package);
        }

        // PUT api/<PackageController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditPackage(int id, [FromBody] PackageDto packageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingPackage = await _unitOfWork.Package.GetByIdAsync(id);
            if(existingPackage == null)
            {
                return NotFound();
            }

            _mapper.Map(packageDto, existingPackage);
            await _unitOfWork.Package.UpdateAsync(existingPackage);
            await _unitOfWork.SaveCompleted();

            return Ok(existingPackage);
        }

        // DELETE api/<PackageController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(int id)
        {
            var package = await _unitOfWork.Package.GetByIdAsync(id);
            if (package == null)
            {
                return NotFound();
            }

            await _unitOfWork.Package.DeleteAsync(id);
            await _unitOfWork.SaveCompleted();

            return Ok(new {message = "Package Deleted Successfully!" });
        }
    }
}
