using EcoPowerHub.UOW;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcoPowerHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("Users")]
     //   [Authorize(Policy = "Only Admin")]
        public async Task<IActionResult> Get()
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var reponse = await _unitOfWork.Users.GetAllUsersAsync();
            if(reponse.IsSucceeded)
                return Ok(reponse);
            return StatusCode(reponse.StatusCode,reponse.Message);
        }


        [HttpGet("Me")]
        //   [Authorize(Policy = "Only Admin")]
        public async Task<IActionResult> GetCurrentUser()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reponse = await _unitOfWork.Users.GetCurrentUserAsync();
            if (reponse.IsSucceeded)
                return Ok(reponse);
            return StatusCode(reponse.StatusCode, reponse.Message);
        }
    }
}
