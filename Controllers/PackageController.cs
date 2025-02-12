using EcoPowerHub.Repositories.Interfaces;
using EcoPowerHub.UOW;
using Microsoft.AspNetCore.Mvc;
using EcoPowerHub.DTO;
using EcoPowerHub.Repositories.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using EcoPowerHub.Models;
using AutoMapper;

namespace EcoPowerHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        
        private readonly IPackageRepository _packageRepository;
        public PackageController( IPackageRepository packageRepository)
        {
            
            _packageRepository = packageRepository;
        }

        // GET: api/<PackageController>
        [HttpGet]
        public async Task<IActionResult> GetAllPackages()
        {
            var response = await _packageRepository.GetAllAsync();
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, response);
        }

        // GET api/<PackageController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackageById(int id)
        {
            var response = await _packageRepository.GetById(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/Package/company/{companyId}
        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetPackagesByCompanyId(int companyId)
        {
            var response = await _packageRepository.GetPackagesByCompanyId(companyId);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, response);
        }

        // POST api/<PackageController>
        [HttpPost]
        //[Authorize(Roles ="Company")]
        //[AllowAnonymous]
        public async Task<IActionResult> CreatePackage([FromBody] PackageDto packageDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var package = _mapper.Map<Package>(packageDto);
            var response= await _packageRepository.AddAsync(packageDto);

            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        // PUT api/<PackageController>/5
        [HttpPut("{id}")]
        //[Authorize(Roles ="Company")]
        public async Task<IActionResult> EditPackage( [FromBody] PackageDto packageDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            

            //var package = _mapper.Map<Package>(packageDto);
            var response = await _packageRepository.UpdateAsync(packageDto);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        // DELETE api/<PackageController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="Company")]
        public async Task<IActionResult> DeletePackage(int id)
        {
            var response = await _packageRepository.DeleteAsync(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
    }
}
