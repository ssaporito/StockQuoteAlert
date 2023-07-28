namespace StockMonitorService
{
    public class StockMonitorWorker : BackgroundService
    {
        private readonly ILogger<StockMonitorWorker> _logger;
        private readonly IStockMonitor _stockMonitor;
        private readonly IStockMonitorBroker _stockMonitorBroker;

        public StockMonitorWorker(ILogger<StockMonitorWorker> logger, IStockMonitor stockMonitor, IStockMonitorBroker stockMonitorBroker)
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

                _stockMonitorBroker.CheckMonitorRequests();

                var stockAlerts = _stockMonitor.MonitorRegisteredStocks();
                Parallel.ForEach(stockAlerts, alert =>
                {
                    _stockMonitorBroker.AlertStockQuote(alert);
                });
                
                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}