﻿using EcoPowerHub.DTO.OrderDto;
using EcoPowerHub.DTO.PackageDto;
using EcoPowerHub.UOW;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcoPowerHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
      
        //<OrderController>
 //       [Authorize(Policy = "Only Admin")]
        [HttpGet("Orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Orders.GetAllOrders();
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        // GET api/<OrderController>/5
   //     [Authorize(Policy = "Only Admin")]
        [HttpGet("OrderById/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Orders.GetOrderById(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        // POST api/<OrderController>
        [HttpPost("Checkout")]
        public async Task<IActionResult> Checkout([FromBody] string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _unitOfWork.Orders.Checkout(userId);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
        [HttpPost("CheckoutPackage")]
        public async Task<IActionResult> CheckoutPackage([FromBody] CheckoutPackageDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Orders.CheckoutPackage(dto);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
        // DELETE api/<OrderController>/5
        //   [Authorize(Policy = "Only Admin")]
        [HttpDelete("DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Orders.DeleteOrder(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

  //      [Authorize(Policy = "Only Admin")]
        [HttpGet("OrderByCompanyId/{id}")]
        public async Task<IActionResult> GetOrderByCompanyId(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Orders.GetOrdersByCompanyId(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [Authorize(Policy = "Only Admin")]
        [HttpGet("GetOrderByCompanyName")]
        public async Task<IActionResult> GetOrderByCompanyName([FromQuery]string companyName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Orders.GetOrdersByCompanyName(companyName);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

     //   [Authorize(Policy = "Only Admin")]
        [HttpGet("OrderByUserId/{userId}")]
        public async Task<IActionResult> GetOrderByUserId(string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Orders.GetOrdersByUserId(userId);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
    }
}


