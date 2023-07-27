namespace StockMonitorService
{
    public class StockMonitorWorker : BackgroundService
    {
        private readonly ILogger<StockMonitorWorker> _logger;        
        private readonly IStockMonitor _stockMonitor;

        public StockMonitorWorker(ILogger<StockMonitorWorker> logger, IStockMonitor stockMonitorService)
        {
            _logger = logger;
            _stockMonitor = stockMonitorService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var result = await _stockMonitor.QueryStockQuote("Test");
                _logger.LogInformation("API Result: {result}", result);
                await Task.Delay(12000, stoppingToken);
            }
        }
    }
}