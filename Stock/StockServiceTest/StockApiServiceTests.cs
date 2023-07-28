using Microsoft.Extensions.Options;
using StockMonitorService;

namespace StockMonitorServiceTest
{
    public class StockApiServiceTests
    {
        private readonly HttpClient _httpClient;
        private readonly StockApiSettings _settings;
        private readonly IStockApiService _stockApiService;        

        public StockApiServiceTests()
        {
            _httpClient = new HttpClient();
            _settings = new StockApiSettings() 
            {
                Endpoint = "https://www.alphavantage.co/query",
                ApiKey = "demo"
            };
            _stockApiService = new StockApiService(_httpClient, Options.Create(_settings));
        }

        [Fact]
        public async Task StockMonitor_FetchesJsonObject()
        {
            var result = await _stockApiService.QueryStockQuote("IBM", "");
            Assert.NotNull(result);
            Assert.Contains(result.Keys, k => k.Contains("price"));
            Assert.Contains(result.Keys, k => k.Contains("change"));
        }
    }
}
