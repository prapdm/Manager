using FluentEmail.Core;
using Manager.Models;
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

            var email = _email
                           .To(dto.Email, $"{dto.Name} {dto.Surname}" )
                           .BCC(_mailSettings.AdminBCC)
                           .Subject(dto.Subject)
                           .UsingTemplateFromFile($@"Views/Mail/{dto.Template}", dto);

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
