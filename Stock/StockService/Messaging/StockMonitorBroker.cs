using Common.Dtos.Stock;
using Common.Helpers;
using Messaging.MessageQueueService;
using StockMonitorService.StockMonitor;
using System.Text.Json;

namespace StockMonitorService.Messaging
{
    public class StockMonitorBroker : IStockMonitorBroker
    {
        private readonly IStockMonitor _stockMonitor;
        private readonly IMessageQueueService _mqService;
        private readonly string _monitorQueueName = QueueNames.MonitorQueue;
        private readonly string _alertQueueName = QueueNames.AlertQueue;

        public StockMonitorBroker(IStockMonitor stockMonitor, IMessageQueueService mqService)
        {
            _stockMonitor = stockMonitor;
            _mqService = mqService;
        }

        public void ConsumeMonitorRequests()
        {
            DeclareQueues();

            _mqService.ConsumeQueue(_monitorQueueName, async message =>
            {
                Console.WriteLine("Received {0}", message);
                var stockToMonitor = JsonSerializer.Deserialize<StockMonitorRequest>(message);
                _stockMonitor.SetMonitoring(stockToMonitor);
            });
        }

        public void PublishStockAlert(StockAlert stockAlert)
        {
            string jsonAlert = JsonSerializer.Serialize(stockAlert);
            _mqService.PublishMessage(_alertQueueName, jsonAlert);
        }

        private void DeclareQueues()
        {
            _mqService.DeclareQueue(_monitorQueueName, _monitorQueueName);
            _mqService.DeclareQueue(_alertQueueName, _alertQueueName);
        }
    }

}
