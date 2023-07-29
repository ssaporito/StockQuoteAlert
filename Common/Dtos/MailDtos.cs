namespace Common.Dtos.Mail
{
    public record EmailMessage(string Subject, string Body, string SenderName, string SenderEmail, string RecipientName, string RecipientEmail);
    
    public class MailInfo
    {
        public string SenderName { get; set; } = default!;
        public string SenderEmail { get; set; } = default!;
        public string RecipientName { get; set; } = default!;
        public string RecipientEmail { get; set; } = default!;
    }
}
