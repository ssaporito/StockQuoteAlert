using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace StockMonitorService
{
    public class StockMonitorService : IStockMonitorService
    {
        private readonly StockApiSettings _stockApisettings;
        private readonly HttpClient _httpClient;        

        public StockMonitorService(HttpClient httpClient, IOptions<StockApiSettings> stockApiSettings) 
        {
            _httpClient = httpClient;
            _stockApisettings = stockApiSettings.Value;
        }

        public async Task<decimal> QueryStockQuote(string stockName, string suffix = ".SA")
        {
            var queryParams = new List<KeyValuePair<string, string>> 
            {
                new("function", "GLOBAL_QUOTE"),
                new("symbol", stockName + suffix),
                new("apikey", _stockApisettings.ApiKey)
            };

            string completeQuery = BuildQuery(_stockApisettings.Endpoint, queryParams);
            dynamic dynamicResponse = await _httpClient.GetFromJsonAsync<dynamic>(completeQuery);
            
            return 0;
        }

        public async Task SendBuyAlert(string stockName, decimal price)
        {

        }

        public async Task SendSellAlert(string stockName, decimal price)
        {

        }

        private string BuildQuery(string baseUrl, List<KeyValuePair<string, string>> queryParams)
        {
            string separator = queryParams.Any() ? "?" : "";
            string result = baseUrl + string.Join("&", queryParams.Select(kvp => kvp.Key + "=" + kvp.Value));
            return result;
        }        
    }
}
