 

namespace Manager.Models
{
    public class MailSenderDto 
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Template { get; set; }
        public string Subject { get; set; }
        public string PlainBody { get; set; }
        public int VerifcationCode { get; set; }
        public string VerifcationToken { get; set; }
        public string Url { get; set; }

    }
}
