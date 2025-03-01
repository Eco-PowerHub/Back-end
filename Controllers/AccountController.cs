using EcoPowerHub.DTO;
using EcoPowerHub.DTO.OTPDto;
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
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Accounts.RegisterAsync(registerDto);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto Dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Accounts.LoginAsync(Dto);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout([FromBody] LoginDto Dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Accounts.Logout(Dto);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordSettingDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Accounts.ChangePasswordAsync(dto);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordSettingDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Accounts.ResetPasswordAsync(dto);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
        [HttpPut("EditProfile")]
        public async Task<IActionResult> EditProfile([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Accounts.updateProfile(userDto);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
        [HttpDelete("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Accounts.DeleteProfileAsync(dto);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
        [HttpPost("GetRefreshToken")]
        public async Task<IActionResult> NewRefreshToken([FromBody] string email)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Accounts.GetRefreshTokenAsync(email);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
        [HttpPost("RevokeRefreshToken")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] string email)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var token = await _unitOfWork.Accounts.RevokeRefreshTokenAsync(email);
            if (!token)
                return NotFound("User or token not found!");
            return Ok("Token revoked successfully");
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOTP([FromBody] SendOtpRequest request)
        {
            var result = await _unitOfWork.Accounts.SendOTPAsync(request.Email);
            if(result.IsSucceeded)
                return Ok(result);
            return StatusCode(result.StatusCode, new { result.Message });
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTP otpRequest)
        {
            var result = await _unitOfWork.Accounts.verifyOTPRequest(otpRequest);
            if(result.IsSucceeded)
                return Ok(result);

            return StatusCode(result.StatusCode , new {result.Message});
        }
    }
}

