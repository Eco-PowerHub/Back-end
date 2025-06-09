using EcoPowerHub.DTO.CartDto;
using EcoPowerHub.Models;
using EcoPowerHub.UOW;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace EcoPowerHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartItemController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

  //      [Authorize(Policy = "Only Admin")]
        [HttpGet("Items")]
        public async Task<IActionResult> GetItems()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.CartItems.GetAllItems();
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpGet("UserItem")]
        public async Task<IActionResult> UserItem()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.CartItems.GetCurrentUserCartItemsAsync();
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpPost("AddItem")]
        public async Task<IActionResult> AddNewItem(CartItemDto cartItem)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.CartItems.AddToCart(cartItem);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpPut("UpdateItem")]
        public async Task<IActionResult> UpdateItem(int id, CartItem cartItem)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.CartItems.UpdateItem(id, cartItem);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

//        [Authorize(Policy = "Only Admin")]
        [HttpDelete("DeleteItem/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.CartItems.DeleteItem(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
    }
}
