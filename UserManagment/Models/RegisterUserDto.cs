﻿
namespace Manager.Models
{
    public class RegisterUserDto
    {
        public int RoleId { get; set; } = 3;
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ConfirmPassword { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

    }
}
