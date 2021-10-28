

using System.ComponentModel.DataAnnotations;

namespace Manager.Models
{
    public class VeryfiyEmailDto
    {   
        [Required]
        public int VerifcationCode { get; set; }
        public int VerifcationToken { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
