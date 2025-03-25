using EcoPowerHub.DTO.UserPropertyDto;
using EcoPowerHub.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcoPowerHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public PropertyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("AddProperty")]
        public async Task<IActionResult> AddUserProerty(PackageOrderDto packageOrderDto)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);
            var response = await _unitOfWork.Properties.AddProperty(packageOrderDto);
            if(response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode,response.Data);
        }

    }
}
