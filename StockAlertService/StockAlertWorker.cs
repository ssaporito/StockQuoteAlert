using Common.Dtos.Stock;
using Microsoft.Extensions.Hosting;
using StockAlertService.Messaging;
using System.Security.Cryptography.X509Certificates;

namespace StockAlertService
{
    public class StockAlertWorker : BackgroundService
    {
        private readonly ILogger<StockAlertWorker> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IStockAlertBroker _stockAlertBroker;
        private readonly string _stockArgName = "STOCK";
        private readonly string _sellArgName = "SELL";
        private readonly string _buyArgName = "BUY";

        public StockAlertWorker(ILogger<StockAlertWorker> logger, IHostApplicationLifetime hostApplicationLifetime, IStockAlertBroker stockAlertBroker)
        {
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;
            _stockAlertBroker = stockAlertBroker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var exeArgsNullable = GetArgs();
            if (!exeArgsNullable.HasValue)
            {
                _hostApplicationLifetime.StopApplication();
            }

            var exeArgs = exeArgsNullable.Value;            
            StockMonitorRequest monitorRequest = new(exeArgs.stockName, exeArgs.buyPrice, exeArgs.sellPrice);
            _stockAlertBroker.DeclareQueues();
            _stockAlertBroker.PublishMonitorRequest(monitorRequest);

            while (!stoppingToken.IsCancellationRequested)
            {
                _stockAlertBroker.ConsumeAlerts();

                await Task.Delay(1000, stoppingToken);
            }
        }

        private (string stockName, decimal sellPrice, decimal buyPrice)? GetArgs()
        {
            string stockName;
            decimal sellPrice, buyPrice;
            string stockArg, sellArg, buyArg;

            var envArgs = Environment.GetEnvironmentVariables();
            var cmdArgs = Environment.GetCommandLineArgs();

            if (envArgs.Contains(_stockArgName) && envArgs.Contains(_sellArgName) && envArgs.Contains(_buyArgName))
            {
                stockArg = (string)envArgs[_stockArgName]!;
                sellArg = (string)envArgs[_sellArgName]!;
                buyArg = (string)envArgs[_buyArgName]!;
            }
            else if (cmdArgs.Length == 4)
            {
                stockArg = cmdArgs[1];
                sellArg = cmdArgs[2];
                buyArg = cmdArgs[3];                                
            } 
            else
            {
                _logger.LogInformation("A aplicação requer os parâmetros stock_name, sell_price e buy_price (e.g. dotnet StockAlertService.dll PETR4 22.67 22.59");
                return null;
            }

            stockName = stockArg;
            bool sellArgOk = decimal.TryParse(sellArg, out sellPrice);
            bool buyArgOk = decimal.TryParse(buyArg, out buyPrice);
            if (!sellArgOk || !buyArgOk)
            {
                _logger.LogInformation("Os parâmetros de compra e venda devem ser valores decimais (e.g. dotnet StockAlertService.dll PETR4 22.67 22.59");
            }

            return (stockName, sellPrice, buyPrice);
        }
    }
}