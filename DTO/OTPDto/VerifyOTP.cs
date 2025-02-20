using System.ComponentModel.DataAnnotations;

namespace EcoPowerHub.DTO.OTPDto
{
    public class VerifyOTP
    {
        [Required]
        public string Email { get; set; }
        public string? OTP { get; set; }
    }
}
