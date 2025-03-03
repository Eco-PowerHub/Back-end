using EcoPowerHub.DTO;
using EcoPowerHub.Repositories.Interfaces;
using EcoPowerHub.UOW;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EcoPowerHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        // GET: api/<ProductController>
        [HttpGet("Products")]
        public async Task<IActionResult> GetAllProducts()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Products.GetAllAsync();
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        
        // GET api/<ProductController>/5
        [HttpGet("ProductById{id}")]
        [Authorize(Policy = "Only Admin")]
        public async Task<IActionResult>  GetProductById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Products.GetById(id);
            if (response.IsSucceeded)
                return Ok(response);

            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpGet("GetProductByName")]

        public async Task<IActionResult> GetProductByName(string name)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Products.GetByName(name);
            if (response.IsSucceeded)
                return Ok(response);

            return StatusCode(response.StatusCode, new { response.Message });
        }
        [HttpGet("ProductsByCategoryId/{categoryId}")]
        [Authorize(Policy = "Only Admin")]
        public async Task<IActionResult> GetProductsByCategoryId(int categoryId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response= await _unitOfWork.Products.GetByCategory(categoryId);
            if(response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpGet("GetProductsByCompanyId/{companyId}")]
        [Authorize(Policy = "Only Admin")]
        public async Task<IActionResult> GetProductsByCompanyId(int companyId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Products.GetByCompany(companyId);
            if(response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
        [HttpGet("SortProductByPrice")]
        public async Task<IActionResult> GetProductsSortedByPrice(int categoryId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Products.GetProductsSortedByPrice(categoryId);
            if(response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }


        // POST api/<ProductController>
        [HttpPost("AddProduct")]
        [Authorize(Policy = "Only Admin")]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto productDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var response= await _unitOfWork.Products.AddAsync(productDto);
            if(response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        // PUT api/<ProductController>/5
        [HttpPut("EditProduct/{id}")]
        [Authorize(Policy = "Company and Admin")]
        public async Task<IActionResult> EditProduct(int id,[FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Products.UpdateAsync(id,productDto);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("DeleteProduct/{id}")]
        [Authorize(Policy = "Only Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Products.DeleteAsync(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpGet("SearchProducts")]
        public async Task<IActionResult> SearchProducts(string searchTerm)
        {
            var response = await _unitOfWork.Products.SearchProductAsync(searchTerm);
            if(response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
    }
}
