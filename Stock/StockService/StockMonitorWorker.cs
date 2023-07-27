namespace StockMonitorService
{
    public class StockMonitorWorker : BackgroundService
    {
        private readonly ILogger<StockMonitorWorker> _logger;        
        private readonly IStockMonitorService _stockMonitorService;

        public StockMonitorWorker(ILogger<StockMonitorWorker> logger, IStockMonitorService stockMonitorService)
        {
            _logger = logger;
            _stockMonitorService = stockMonitorService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var result = await _stockMonitorService.QueryStockQuote("Test");
                _logger.LogInformation("API Result: {result}", result);
                await Task.Delay(12000, stoppingToken);
            }
        }
    }
}