namespace Common.Dtos.Mail
{
    public record EmailMessage(string Subject, string Body, string SenderName, string SenderEmail, string RecipientName, string RecipientEmail);
}
