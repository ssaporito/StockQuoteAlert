using System.Text.Json;
using Common.Dtos.Mail;
using Common.Dtos.Stock;
using Common.Helpers;
using Common.Helpers.Converters;
using Messaging.MessageQueueService;
using Microsoft.Extensions.Options;


namespace StockAlertService.Messaging
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
                var alertEmail = Converters.StockAlertToEmail(stockAlert, _mailInfo);
                PublishMailRequest(alertEmail);
            });
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
