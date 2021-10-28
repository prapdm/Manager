using Manager.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Models
{
    public class UserDto
    {
        public int Id { get; set; } 
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required, NotNull]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public List<Role> Roles { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
