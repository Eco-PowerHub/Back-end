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
            var response = await _unitOfWork.Products.GetAllProductsAsync();
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        
        // GET api/<ProductController>/5
        [HttpGet("GetProductById/{id}")]
        [Authorize(Policy = "Only Admin")]
        public async Task<IActionResult>  GetProductById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Products.GetProductById(id);
            if (response.IsSucceeded)
                return Ok(response);

            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpGet("ProductByName")]
        public async Task<IActionResult> GetProductByName([FromQuery]string name)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Products.GetProductByName(name);
            if (response.IsSucceeded)
                return Ok(response);

            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpGet("ProductsByCategory")]
        public async Task<IActionResult> GetProductsByCategoryId([FromQuery]string categoryName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response= await _unitOfWork.Products.GetProductByCategory(categoryName);
            if(response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpGet("ProductsByCompany")]
        public async Task<IActionResult> GetProductsByCompanyId([FromQuery] string companyName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Products.GetProductByCompany(companyName); 
            if(response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpGet("SortProductByPrice")]
        public async Task<IActionResult> GetProductsSortedByPrice([FromQuery] string categoryName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Products.GetProductsSortedByPrice(categoryName);
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
            var response= await _unitOfWork.Products.AddProductAsync(productDto);
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
            var response = await _unitOfWork.Products.UpdateProductAsync(id,productDto);
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
            var response = await _unitOfWork.Products.DeleteProductAsync(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpGet("SearchProducts")]
        public async Task<IActionResult> SearchProducts([FromQuery]string searchTerm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Products.SearchProductAsync(searchTerm);
            if(response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
    }
}
