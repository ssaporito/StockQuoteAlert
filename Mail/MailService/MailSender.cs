using Common.Dtos.Mail;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MailService
{
    public class MailSender : IMailSender
    {
        private readonly ISmtpClient _smtpClient;
        private readonly SmtpSettings _smtpSettings;

        public MailSender(ISmtpClient smtpClient, IOptions<SmtpSettings> smtpSettings)
        {
            _smtpClient = smtpClient;
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendMail(EmailMessage email)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(email.SenderName, email.SenderEmail));
            message.To.Add(new MailboxAddress(email.RecipientName, email.RecipientEmail));
            message.Subject = email.Subject;
            message.Body = new TextPart("plain") { Text = email.Body };

            await _smtpClient.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, false);
            await _smtpClient.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
            await _smtpClient.SendAsync(message);
            await _smtpClient.DisconnectAsync(true);
        }
    }    
}
