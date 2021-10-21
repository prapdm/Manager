

namespace Manager.Models
{
    public class VeryfiyEmailDto
    {
        public int VerifcationCode { get; set; }
        public int VerifcationToken { get; set; }
        public string Email { get; set; }
    }
}
