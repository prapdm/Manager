using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
