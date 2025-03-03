using EcoPowerHub.DTO;
using EcoPowerHub.Models;
using EcoPowerHub.UOW;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace EcoPowerHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("Companies")]
        [Authorize(Policy = "Only Admin")]
        public async Task<IActionResult> GetAllCompanies()
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Company.GetAllCompany();
            if(response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode , new {response.Message});
        }

        [HttpGet("CompanybyId/{id}")]
        [Authorize(Policy = "Only Admin")]
        public async Task<IActionResult> GetCompanybyId(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Company.GetCompanybyId(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpGet("CompanybyName/{name}")]
        [Authorize(Policy = "Client and Admin")]
        public async Task<IActionResult> CompanybyName(string name)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Company.GetCompanybyName(name);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpPost("AddCompany")]
        [Authorize(Policy = "Only Admin")]
        public async Task<IActionResult> AddCompany(CompanyDto company)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Company.AddCompany(company);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpPut("EditCompany")]
        [Authorize(Policy = "Company and Admin")]
        public async Task<IActionResult> EditCompany(int id ,CompanyDto company)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Company.UpdateCompany(id, company);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
        [HttpDelete("DeleteCompany/{id}")]
        [Authorize(Policy = "Only Admin")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Company.DeleteCompany(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
        [HttpGet("CompanyProducts")]
        public async Task<IActionResult> GetCompanyProducts(int companyId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Company.GetCompanyProducts(companyId);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
        [HttpGet("CompanyPackages")]
        public async Task<IActionResult> GetCompanyPackages(int companyId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Company.GetCompanyPackages(companyId);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
    }
}
