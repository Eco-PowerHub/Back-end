using System.ComponentModel.DataAnnotations;

namespace EcoPowerHub.DTO.UserDto
{
    public class UserDto
    {
        //[Key]
        //public string UserId { get; set; }
        public string Username { get; set; }
        public string PhoneNumder { get; set; }
        public string Email { get; set; }
      

    }
}
