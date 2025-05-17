using CloudinaryDotNet;
using EcoPowerHub.Models;
using EcoPowerHub.UOW;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EcoPowerHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public FeedbackController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/<FeedbackController>
        [HttpGet("Feedbacks")]
    //    [Authorize(Policy = "Client and Admin")]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);
            var response = await _unitOfWork.UserFeedbacks.GetAllFeedbacksAsync();
            if(response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode,new { response.Message });
        }

        // GET api/<FeedbackController>/5
        [HttpGet("FeedbackById/{id}")]
    //    [Authorize(Policy = "Only Admin")]
        public async Task<IActionResult> GetFeedbackById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.UserFeedbacks.GetFeedbackByIdAsync(id);
            if(response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode,new {response.Message});
        }

        // POST api/<FeedbackController>
        [HttpPost("AddFeedback")]
   //     [Authorize(Policy = "Only Client")]
        public async Task<IActionResult> AddFeedback([FromBody] UserFeedBack userFeedBack)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.UserFeedbacks.AddFeedbackAsync(userFeedBack);
            if(response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode,new {response.Message});
        }


        // DELETE api/<FeedbackController>/5
        [HttpDelete("DeleteFeedback/{id}")]
    //    [Authorize(Policy = "Only Admin")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.UserFeedbacks.DeleteFeedbackAsync(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
    }
}
