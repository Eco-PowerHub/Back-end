using EcoPowerHub.Helpers;
using System.ComponentModel.DataAnnotations;

namespace EcoPowerHub.DTO.UserDto
{
    public class RegisterDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Password and Confirmation Passwoed don't match.!!")]
        public string ConfirmNewPassword { get; set; }
        [Required]
        public Roles Role  { get; set; }
        public string OTP { get; set; }
        public DateTime? OTPExpiry { get; set; }

    }
}
