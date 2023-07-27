using MailKit.Net.Smtp;
using MimeKit;

namespace MailService
{
    public class SmtpClientWrapper : ISmtpClient
    {
        private readonly SmtpClient _smtpClient;

        public SmtpClientWrapper()
        {
            _smtpClient = new SmtpClient();
        }

        public Task ConnectAsync(string host, int port, bool useSsl)
        {
            return _smtpClient.ConnectAsync(host, port, useSsl);
        }

        public Task AuthenticateAsync(string user, string password)
        {
            return _smtpClient.AuthenticateAsync(user, password);
        }

        public Task SendAsync(MimeMessage message)
        {
            return _smtpClient.SendAsync(message);
        }

        public Task DisconnectAsync(bool quit)
        {
            return _smtpClient.DisconnectAsync(quit);
        }

        public void Dispose()
        {
            _smtpClient.Dispose();
        }
    }
}
