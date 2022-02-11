namespace Manager.Models
{
    public class ChangePasswordDto
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string VerifyToken { get; set; }
    }
}
