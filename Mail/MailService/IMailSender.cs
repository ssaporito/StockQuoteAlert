namespace MailService
{
    public interface IMailSender
    {
        Task SendMail(EmailMessage message);
    }
}
