using System.ComponentModel.DataAnnotations;

namespace EcoPowerHub.DTO.UserDto
{
    public class VerifyOTPRequest
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string OTP { get; set; } = string.Empty ;
    }
}
