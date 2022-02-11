using System.ComponentModel.DataAnnotations;

namespace Manager.Models
{
    public class LoginUserDto
    {
        [Required]
        public string Password { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
