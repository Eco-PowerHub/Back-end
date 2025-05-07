using System.ComponentModel.DataAnnotations;

namespace EcoPowerHub.DTO.UserDto
{
    public class VerifyOTPRequest
    {
        
        public string Email { get; set; } = string.Empty;
        [Required]
        public string OTP { get; set; } = string.Empty ;
    }
}
