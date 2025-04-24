using EcoPowerHub.Helpers;
using System.ComponentModel.DataAnnotations;

namespace EcoPowerHub.DTO.UserDto
{
    public class LoginDto
    {
        [Required,EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }
    }
}
