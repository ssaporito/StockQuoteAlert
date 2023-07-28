using Common.Dtos.Stock;

namespace StockAlertService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IStockAlertBroker _stockAlertBroker;

        public Worker(ILogger<Worker> logger, IStockAlertBroker stockAlertBroker)
        {
            _logger = logger;
            _stockAlertBroker = stockAlertBroker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var cmdArgs = Environment.GetCommandLineArgs();
            var stockName = cmdArgs[1];
            var sellPrice = decimal.Parse(cmdArgs[2]);
            var buyPrice = decimal.Parse(cmdArgs[3]);
            StockMonitorRequest monitorRequest = new(stockName, buyPrice, sellPrice);
            _stockAlertBroker.DeclareQueues();
            _stockAlertBroker.PublishMonitorRequest(monitorRequest);

            while (!stoppingToken.IsCancellationRequested)
            {
                _stockAlertBroker.ConsumeAlerts();

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}