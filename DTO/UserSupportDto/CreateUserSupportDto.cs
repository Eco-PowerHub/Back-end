using System.ComponentModel.DataAnnotations;

namespace EcoPowerHub.DTO.UserSupportDto
{
    public class CreateUserSupportDto
    {
        //public string UserName { get; set; }
        //[Required(ErrorMessage = "Email is required")]
        //[EmailAddress(ErrorMessage = "Invalid email format")]
        //public string Email { get; set; }
        //public string PhoneNumber { get; set; }
        public string Subject { get; set; }
    }
}
