using EcoPowerHub.Repositories.Interfaces;
using EcoPowerHub.UOW;
using Microsoft.AspNetCore.Mvc;
using EcoPowerHub.DTO;
using EcoPowerHub.Repositories.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using EcoPowerHub.Models;
using AutoMapper;
using EcoPowerHub.DTO.PackageDto;

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

        [HttpGet("PackageById/{id}")]
    //    [Authorize(Policy = "Only Admin")]
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
        public async Task<IActionResult> GetPackagesByCompanyName([FromQuery] string companyName)
        {
            var response = await _unitOfWork.Packages.GetPackagesByCompanyName(companyName);
            if (response.IsSucceeded)
                return Ok(response);

            return StatusCode(response.StatusCode, response);
        }


        [HttpPost("AddPackage")]
   //     [Authorize(Policy = "Company And Admin")]
        public async Task<IActionResult> AddPackage([FromBody] PackageDto packageDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Packages.AddPackageAsync(packageDto);
            if (response.IsSucceeded)
                return Ok(response);

            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpPut("EditPackage/{id}")]
   //     [Authorize(Policy = "Company and Admin")]
        public async Task<IActionResult> EditPackage(int id, [FromBody] PackageDto packageDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Packages.UpdatePackageAsync(id, packageDto);
            if (response.IsSucceeded)
                return Ok(response);

            return StatusCode(response.StatusCode, new { response.Message });
        }


        [HttpDelete("DeletePackage/{id}")]
    //    [Authorize(Policy = "Company and Admin")]
        public async Task<IActionResult> DeletePackage(int id)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Packages.DeletePackageAsync(id);
            if (response.IsSucceeded)
                return Ok(response);

            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpPost("AddpPackageTOCart/{packageId}")]
        public async Task<IActionResult> AddPackageToCart(int packageId,int cartId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Packages.AddPackageToCart(packageId,cartId);
            if (response.IsSucceeded)
                return Ok(response);

            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpGet("UserPackage/{packageId}")]
        public async Task<IActionResult> GetUserPackage()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Packages.GetCurrentUserPackageAsync();
            if (response.IsSucceeded)
                return Ok(response);

            return StatusCode(response.StatusCode, new { response.Message });
        }
    }
}