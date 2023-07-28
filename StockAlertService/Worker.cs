using Common.Dtos.Stock;
using System.Security.Cryptography.X509Certificates;

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
            var exeArgsNullable = GetArgs();
            if (!exeArgsNullable.HasValue)
            {                
                return;
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

            string stockArgName = "ARG1";
            string sellArgName = "ARG2";
            string buyArgName = "ARG3";
            string stockArg, sellArg, buyArg;

            var envArgs = Environment.GetEnvironmentVariables();
            var cmdArgs = Environment.GetCommandLineArgs();

            if (envArgs.Contains(stockArgName) && envArgs.Contains(sellArgName) && envArgs.Contains(buyArgName))
            {
                stockArg = (string)envArgs[stockArgName]!;
                sellArg = (string)envArgs[sellArgName]!;
                buyArg = (string)envArgs[buyArgName]!;
            }
            else if (cmdArgs.Length == 4)
            {
                stockArg = cmdArgs[1];
                sellArg = cmdArgs[2];
                buyArg = cmdArgs[3];                                
            } 
            else
            {
                _logger.LogInformation("A aplica��o requer os par�metros stock_name, sell_price e buy_price (e.g. dotnet StockAlertService.dll PETR4 22.67 22.59");
                return null;
            }

            stockName = stockArg;
            bool sellArgOk = decimal.TryParse(sellArg, out sellPrice);
            bool buyArgOk = decimal.TryParse(buyArg, out buyPrice);
            if (!sellArgOk || !buyArgOk)
            {
                _logger.LogInformation("Os par�metros de compra e venda devem ser valores decimais (e.g. dotnet StockAlertService.dll PETR4 22.67 22.59");
            }

            return (stockName, sellPrice, buyPrice);
        }
    }
}