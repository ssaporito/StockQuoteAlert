using Common.Dtos.Mail;
using Common.Dtos.Stock;

namespace Common.Helpers.Converters
{
    public static class Converters
    {
        public static EmailMessage StockAlertToEmail(StockAlert alert, MailInfo mailInfo)
        {
            string buyOrSell = alert.AlertType == StockAlertType.Buy ? "buy" : "sell";
            string subject = $"It's a good time to {buyOrSell} {alert.MonitorRequest.StockName}";
            string body = $"Stock quote for {alert.MonitorRequest.StockName} has reached the recommended price to {buyOrSell}.";
            EmailMessage alertEmail = new(subject, body, mailInfo.SenderName, mailInfo.SenderEmail, mailInfo.RecipientName, mailInfo.RecipientEmail);
            return alertEmail;
        }
    }
}
