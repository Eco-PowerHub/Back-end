﻿namespace EcoPowerHub.DTO.UserDto.PasswordSettingDto
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string confirmNewPassword { get; set; }


    }
}
