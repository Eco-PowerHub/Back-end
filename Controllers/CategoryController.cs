using EcoPowerHub.DTO;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.Interfaces;
using EcoPowerHub.UOW;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EcoPowerHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        // GET: api/<CategoryController>
        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Categories.GetAllAsync();
            if(response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode,new { response.Message });
        }

        // GET api/<CategoryController>/5
        [HttpGet("GetCategoryById/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var response = await _unitOfWork.Categories.GetById(id);
            if (response.IsSucceeded)
                return Ok(response);

            return StatusCode(response.StatusCode, new { response.Message });
        }

        // GET api/<CategoryController>/CategoryName
        [HttpGet("GetCategoryByName/{name}")]
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            var response = await _unitOfWork.Categories.GetByName(name);
            if (response.IsSucceeded)
                return Ok(response);

            return StatusCode(response.StatusCode, new { response.Message });
        }

        // POST api/<CategoryController>
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseDto
                {
                    Message = "Invalid request data.",
                    IsSucceeded = false,
                    StatusCode = 400
                });

            var response = await _unitOfWork.Categories.AddAsync(categoryDto);
            if (response.IsSucceeded)
                return Ok(response);

            return StatusCode(response.StatusCode, new { response.Message });
        }

        // PUT api/<CategoryController>/5
        [HttpPut("EditCategory/{id}")]
        public async Task<IActionResult> EditCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseDto
                {
                    Message = "Invalid request data.",
                    IsSucceeded = false,
                    StatusCode = 400
                });

            var response = await _unitOfWork.Categories.UpdateAsync(id,categoryDto);
            if (response.IsSucceeded)
                return Ok(response);

            return StatusCode(response.StatusCode, new { response.Message });
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var response = await _unitOfWork.Categories.DeleteAsync(id);
            if (response.IsSucceeded)
                return Ok(response);

            return StatusCode(response.StatusCode, new { response.Message });
        }
    }
}
