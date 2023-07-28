using Common.Dtos.Mail;

namespace MailService.MailSender
{
    public interface IMailSender
    {
        Task SendMail(EmailMessage message);
    }
}
