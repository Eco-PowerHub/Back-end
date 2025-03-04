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

        private readonly IUnitOfWork _unitOfWork;
        public PackageController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/<PackageController>
        [HttpGet("Packages")]
        public async Task<IActionResult> GetAllPackages()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Packages.GetAllPackagesAsync();
            if (response.IsSucceeded)
                return Ok(response);

            return StatusCode(response.StatusCode, new { response.Message });
        }

        // GET api/<PackageController>/5
        [HttpGet("PackageById/{id}")]
        [Authorize(Policy = "Only Admin")]
        public async Task<IActionResult> GetPackageById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Packages.GetPackageById(id);
            if (response.IsSucceeded)
                return Ok(response);

            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpGet("PackagesByCompany")]
        public async Task<IActionResult> GetPackagesByCompanyName([FromQuery]string companyName)
        {
            var response = await _unitOfWork.Packages.GetPackagesByCompanyName(companyName);
            if (response.IsSucceeded)
                return Ok(response);

            return StatusCode(response.StatusCode, response);
        }

        // POST api/<PackageController>
        [HttpPost("AddPackage")]
        [Authorize(Policy = "Company And Admin")]
        public async Task<IActionResult> AddPackage([FromBody] PackageDto packageDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Packages.AddPackageAsync(packageDto);
            if (response.IsSucceeded)
                return Ok(response);

            return StatusCode(response.StatusCode, new { response.Message });
        }

        // PUT api/<PackageController>/5
        [HttpPut("EditPackage/{id}")]
        [Authorize(Policy = "Company and Admin")]
        public async Task<IActionResult> EditPackage(int id, [FromBody] PackageDto packageDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Packages.UpdatePackageAsync(id, packageDto);
            if (response.IsSucceeded)
                return Ok(response);

            return StatusCode(response.StatusCode, new { response.Message });
        }

        // DELETE api/<PackageController>/5
        [HttpDelete("DeletePackage/{id}")]
        [Authorize(Policy = "Company and Admin")]
        public async Task<IActionResult> DeletePackage(int id)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Packages.DeletePackageAsync(id);
            if (response.IsSucceeded)
                return Ok(response);

            return StatusCode(response.StatusCode, new { response.Message });
        }
    }
}