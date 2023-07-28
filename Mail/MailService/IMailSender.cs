using Common.Dtos.Mail;

namespace MailService
{
    public interface IMailSender
    {
        Task SendMail(EmailMessage message);
    }
}
