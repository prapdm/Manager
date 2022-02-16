using FluentEmail.Core;
using Manager.Models;
using System.IO;
using System.Threading.Tasks;

namespace Manager.Services
{
    public interface IMailSenderService
    {
        Task SendHtml(MailSenderDto dto);
        Task SendPlain(MailSenderDto dto);
    }

    public class MailSenderService : IMailSenderService
    {
        private readonly IFluentEmail _email;
        private readonly MailSettings _mailSettings;

        public MailSenderService(IFluentEmail email, MailSettings mailSettings)
        {
            _email = email;
            _mailSettings = mailSettings;
        }
        public async Task SendHtml(MailSenderDto dto)
        {
            string template = "";
            switch (dto.Template)
            {
                case "ResetPassword":
                   template+= "To reset password please clik link:<br /><a href=\"@Model.Url\">@Model.Url</a>";
                    break;
                case "Welcome":
                    template += "<h1>Hi @Model.Name, </h1>Activation Code: <b> @Model.VerifcationCode </b>";
                    break;
                default:
                    template += "";
                    break;

            }  



            template+= "<br><br>Copy or rewrite the code, then return to the browser to activate account. <br><br>If you need help, do not hesitate to contact us.<br>This message was generated automatically.Please do not reply.<br><br><br> Kind regards,<br>Administrator<br><br>";

            var email = _email
                           .To(dto.Email, $"{dto.Name} {dto.Surname}" )
                           .BCC(_mailSettings.AdminBCC)
                           .Subject(dto.Subject)
                           .UsingTemplate(template, dto);
            
            await email.SendAsync();
        }

        public async Task SendPlain(MailSenderDto dto)
        {

            var email = _email
                           .To(dto.Email, $"{dto.Name} {dto.Surname}")
                           .BCC(_mailSettings.AdminBCC)
                           .Subject(dto.Subject)
                           .Body(dto.PlainBody);

            await email.SendAsync();
        }
    }
}
