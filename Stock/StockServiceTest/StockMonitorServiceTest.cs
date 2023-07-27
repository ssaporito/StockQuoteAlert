using Microsoft.Extensions.Options;
using StockMonitorService;
using Moq;

namespace StockQuoteAlertApiTest
{
    public class StockMonitorServiceTest
    {
        private readonly StockMonitor _stockMonitorService;

        public StockMonitorServiceTest()
        {
            var httpClient = new HttpClient();
            var stockApiSettings = new StockApiSettings { Endpoint = "https://www.alphavantage.co/query", ApiKey = "demo" };
            _stockMonitorService = new StockMonitor(httpClient, Options.Create(stockApiSettings));
        }

        [Fact]
        public async Task StockMonitor_FetchesJsonObject()
        {
            var result = await _stockMonitorService.QueryStockQuote("IBM", "");
            Assert.NotNull(result);
            Assert.Contains(result, kv => kv.Key.Contains("price"));
            Assert.Contains(result, kv => kv.Key.Contains("change"));
        }
    }
}