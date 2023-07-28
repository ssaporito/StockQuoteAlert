using Common.Dtos.Stock;
using Messaging;
using System.Text.Json;

namespace StockMonitorService
{
    public class StockMonitorBroker : IStockMonitorBroker
    {
        private readonly IStockMonitor _stockMonitor;
        private readonly IMessageQueueService _mqService;
        private readonly string _monitorQueueName = "stock_monitor";
        private readonly string _alertQueueName = "quote_alert";

        public StockMonitorBroker(IStockMonitor stockMonitor, IMessageQueueService mqService)
        {
            _stockMonitor = stockMonitor;
            _mqService = mqService;
        }        

        public void CheckMonitorRequests()
        {
            DeclareQueues();

            _mqService.ConsumeQueue(_monitorQueueName, async message =>
            {
                Console.WriteLine("Received {0}", message);
                var stockToMonitor = JsonSerializer.Deserialize<StockMonitorRequest>(message);
                _stockMonitor.SetMonitoring(stockToMonitor);
            });
        }

        public void AlertStockQuote(StockAlert stockAlert)
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
