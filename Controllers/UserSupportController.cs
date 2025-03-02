using EcoPowerHub.DTO.UserSupportDto;
using EcoPowerHub.Models;
using EcoPowerHub.UOW;
using Microsoft.AspNetCore.Mvc;


namespace EcoPowerHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSupportController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserSupportController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/<UserSupportController>
        [HttpGet("GetAllSupports")]
        public async Task<IActionResult> GetAllSupports()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Supports.GetAllSupportsAsync();
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        // GET api/<UserSupportController>/5
        [HttpGet("GetSupportById/{id}")]
        public async Task<IActionResult> GetSupportById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Supports.GetSupportByIdAsync(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        // POST api/<UserSupportController>
        [HttpPost("AddSupport")]
        public async Task<IActionResult> AddSupport([FromBody] CreateUserSupportDto supportDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Supports.AddSupportAsync(supportDto);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        // PUT api/<UserSupportController>/5
        [HttpPut("AddResponse/{id}")]
        public async Task<IActionResult> AddResponse(int id, [FromBody] AddResponseDto responseDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Supports.AddResponseAsync(id, responseDto);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        // DELETE api/<UserSupportController>/5
        [HttpDelete("DeleteSupport{id}")]
        public async Task<IActionResult> DeleteSupport(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Supports.DeleteSupportAsync(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
    }
}
