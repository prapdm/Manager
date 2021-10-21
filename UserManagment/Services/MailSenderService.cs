using FluentEmail.Core;
using Manager.Models;

namespace Manager.Services
{
    public interface IMailSenderService
    {
        void SendHtml(MailSenderDto dto);
        void SendPlain(MailSenderDto dto);
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
        public async void SendHtml(MailSenderDto dto)
        {

            var email = _email
                           .To(dto.Email, $"{dto.Name} {dto.Surname}" )
                           .BCC(_mailSettings.AdminBCC)
                           .Subject(dto.Subject)
                           .UsingTemplateFromFile($@"Views\Mail\{dto.Template}", dto);

            await email.SendAsync();
        }

        public async void SendPlain(MailSenderDto dto)
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
