using EcoPowerHub.DTO.UserDto;
using EcoPowerHub.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcoPowerHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);
            var response = await _unitOfWork.Accounts.RegisterAsync(registerDto);
            if(response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode,new {response.Message});
        }
    }
}
