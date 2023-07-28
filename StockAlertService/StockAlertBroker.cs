using System.Text.Json;
using Common.Dtos.Mail;
using Common.Dtos.Stock;
using Common.Helpers;
using Messaging;
using Microsoft.Extensions.Options;
using StockAlertService.Dtos;


namespace StockAlertService
{
    public class StockAlertBroker : IStockAlertBroker
    {
        private readonly MailInfo _mailInfo;
        private readonly IMessageQueueService _mqService;
        private readonly string _monitorQueueName = QueueNames.MonitorQueue;
        private readonly string _alertQueueName = QueueNames.AlertQueue;
        private readonly string _mailQueueName = QueueNames.SendMailQueue;

        public StockAlertBroker(IOptions<MailInfo> mailInfo, IMessageQueueService mqService)
        {
            _mailInfo = mailInfo.Value;
            _mqService = mqService;            
        }

        public void ConsumeAlerts()
        {
            DeclareQueues();

            _mqService.ConsumeQueue(_alertQueueName, async message =>
            {
                Console.WriteLine("Received {0}", message);
                var stockAlert = JsonSerializer.Deserialize<StockAlert>(message);
                var alertEmail = StockAlertToEmail(stockAlert);
                PublishMailRequest(alertEmail);
            });
        }

        private EmailMessage StockAlertToEmail(StockAlert alert)
        {
            string buyOrSell = alert.AlertType == StockAlertType.Buy ? "buy" : "sell";
            string subject = $"It's a good time to {buyOrSell} {alert.MonitorRequest.StockName}";
            string body = $"Stock quote for {alert.MonitorRequest.StockName} has reached the recommended price to {buyOrSell}.";
            EmailMessage alertEmail = new(subject, body, _mailInfo.SenderName, _mailInfo.SenderEmail, _mailInfo.RecipientName, _mailInfo.RecipientEmail);
            return alertEmail;
        }

        public void PublishMailRequest(EmailMessage alertEmail)
        {
            string jsonAlert = JsonSerializer.Serialize(alertEmail);
            _mqService.PublishMessage(_mailQueueName, jsonAlert);
        }

        public void PublishMonitorRequest(StockMonitorRequest monitorRequest)
        {
            string json = JsonSerializer.Serialize(monitorRequest);
            _mqService.PublishMessage(_monitorQueueName, json);
        }

        public void DeclareQueues()
        {
            _mqService.DeclareQueue(_monitorQueueName, _monitorQueueName);
            _mqService.DeclareQueue(_alertQueueName, _alertQueueName);
            _mqService.DeclareQueue(_mailQueueName, _mailQueueName);
        }
    }
}
