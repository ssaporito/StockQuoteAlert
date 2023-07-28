using StockMonitorService.Messaging;
using StockMonitorService.StockMonitor;

namespace StockMonitorService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IStockMonitor _stockMonitor;
        private readonly IStockMonitorBroker _stockMonitorBroker;

        public Worker(ILogger<Worker> logger, IStockMonitor stockMonitor, IStockMonitorBroker stockMonitorBroker)
        {
            _logger = logger;
            _stockMonitor = stockMonitor;
            _stockMonitorBroker = stockMonitorBroker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                _stockMonitorBroker.ConsumeMonitorRequests();

                var stockAlerts = _stockMonitor.MonitorRegisteredStocks();
                Parallel.ForEach(stockAlerts, alert =>
                {
                    _stockMonitorBroker.PublishStockAlert(alert);
                });
                
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}