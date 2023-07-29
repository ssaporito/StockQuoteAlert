using Common.Dtos.Stock;
using StockMonitorService.StockMonitor;
using MailService.MailSender;
using Common.Helpers.Converters;
using Common.Dtos.Mail;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace StockAlertAppSimple
{
    public class StockAlertAppSimpleWorker : BackgroundService
    {
        private readonly ILogger<StockAlertAppSimpleWorker> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IStockMonitor _stockMonitor;
        private readonly IMailSender _mailSender;
        private readonly MailInfo _mailInfo;        

        public StockAlertAppSimpleWorker(ILogger<StockAlertAppSimpleWorker> logger, IHostApplicationLifetime hostApplicationLifetime, IStockMonitor stockMonitor, IMailSender mailSender, IOptions<MailInfo> mailOptions)
        {
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;
            _stockMonitor = stockMonitor;
            _mailSender = mailSender;
            _mailInfo = mailOptions.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string stockArg, sellArg, buyArg;
            string stockName;
            decimal sellPrice, buyPrice;

            var cmdArgs = Environment.GetCommandLineArgs();
            try
            {
                stockArg = cmdArgs[1];
                sellArg = cmdArgs[2];
                buyArg = cmdArgs[3];

                stockName = stockArg;
                sellPrice = sellArg.ToCurrencyDecimal();
                buyPrice = buyArg.ToCurrencyDecimal();

                StockMonitorRequest monitorRequest = new(stockName, buyPrice, sellPrice);
                _stockMonitor.SetMonitoring(monitorRequest);
                _logger.LogInformation($"Monitorando ativo {stockName} com venda recomendada a {sellPrice.ToCurrencyString()} e compra recomendada a {buyPrice.ToCurrencyString()}");
            }
            catch
            {
                _logger.LogInformation("A aplicação requer os parâmetros stock_name, sell_price e buy_price (e.g. dotnet StockAlertService.dll PETR4 22.67 22.59");
                _hostApplicationLifetime.StopApplication();
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                var stockAlerts = _stockMonitor.MonitorRegisteredStocks();
                foreach (var alert in stockAlerts)
                {
                    string transaction = alert.AlertType == StockAlertType.Buy ? "compra" : "venda";
                    string assetName = alert.MonitorRequest.StockName;
                    _logger.LogInformation($"Ativo {assetName} atingiu preço recomendado para {transaction} a {alert.MonitorData.Price.ToCurrencyString()}");
                    var email = Converters.StockAlertToEmail(alert, _mailInfo);
                    await _mailSender.SendMail(email);
                    _logger.LogInformation($"E-mail enviado para recomendação de {transaction} do ativo {assetName}");
                }
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}