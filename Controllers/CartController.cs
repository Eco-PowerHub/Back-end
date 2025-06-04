using EcoPowerHub.DTO.CartDto;
using EcoPowerHub.UOW;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcoPowerHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

  //      [Authorize(Policy = "Only Admin")]
        [HttpGet("Carts")]
        public async Task<IActionResult> GetCarts()
        {
            if (ModelState.IsValid)
            {
                var response = await _unitOfWork.Carts.GetAllCart();
                if (response.IsSucceeded)
                    return Ok(response);
                return StatusCode(response.StatusCode, response.Message);
            }
            return BadRequest(ModelState);

        }

        [HttpPost("AddCart")]
        public async Task<IActionResult> AddCart()
        {
            if (ModelState.IsValid)
            {
                var response = await _unitOfWork.Carts.AddCart();
                if (response.IsSucceeded)
                    return Ok(response);
                return StatusCode(response.StatusCode, response.Message);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("UpdateCart")]
        public async Task<IActionResult> UpdateCart(int id, CartDto cart)
        {
            if (ModelState.IsValid)
            {
                var response = await _unitOfWork.Carts.UpdateCart(id, cart);
                if (response.IsSucceeded)
                    return Ok(response);
                return StatusCode(response.StatusCode, response.Message);
            }
            return BadRequest(ModelState);
        }

 //       [Authorize(Policy = "Only Admin")]
        [HttpDelete("DeleteCart/{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            if (ModelState.IsValid)
            {
                var response = await _unitOfWork.Carts.DeleteCart(id);
                if (response.IsSucceeded)
                    return Ok(response);
                return StatusCode(response.StatusCode, response.Message);
            }
            return BadRequest(ModelState);
        }
    }
}
