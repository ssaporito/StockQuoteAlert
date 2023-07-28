namespace MailService.Smtp
{
    public class SmtpSettings
    {
        public string Server { get; set; } = default!;
        public int Port { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
