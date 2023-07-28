namespace StockAlertService.Dtos
{
    public class MailInfo
    {
        public string SenderName { get; set; } = default!;
        public string SenderEmail { get; set; } = default!;
        public string RecipientName { get; set; } = default!;
        public string RecipientEmail { get; set;} = default!;
    }
}
