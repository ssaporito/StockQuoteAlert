using MimeKit;

namespace MailService
{
    public interface ISmtpClient : IDisposable
    {
        Task ConnectAsync(string host, int port, bool useSsl);
        Task AuthenticateAsync(string user, string password);
        Task SendAsync(MimeMessage message);
        Task DisconnectAsync(bool quit);
    }    
}
