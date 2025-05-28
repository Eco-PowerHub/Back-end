using EcoPowerHub.Helpers;

namespace EcoPowerHub.DTO.UserDto
{
    public class UserDataDto
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public Roles Role { get; set; }

    }
}
