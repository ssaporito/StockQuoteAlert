using Common.Dtos.Mail;
using Common.Dtos.Stock;
using System.Globalization;

namespace Common.Helpers.Converters
{
    public static class Converters
    {
        public static EmailMessage StockAlertToEmail(StockAlert alert, MailInfo mailInfo)
        {
            string buyOrSell = alert.AlertType == StockAlertType.Buy ? "compra" : "venda";
            string subject = $"É um bom momento para {buyOrSell} de {alert.MonitorRequest.StockName}";            
            string body = $"A cotação do ativo {alert.MonitorRequest.StockName} chegou ao preço recomendado de {buyOrSell} com valor {alert.MonitorData.Price.ToCurrencyString()}";
            EmailMessage alertEmail = new(subject, body, mailInfo.SenderName, mailInfo.SenderEmail, mailInfo.RecipientName, mailInfo.RecipientEmail);
            return alertEmail;
        }

        public static string ToCurrencyString(this decimal value)
        {
            return value.ToString("C", CultureInfo.GetCultureInfo("en-US"));
        }

        public static decimal ToCurrencyDecimal(this string value)
        {
            decimal result = decimal.Parse(value, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));
            return result;
        }
    }
}
