using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace StockMonitorService
{
    public class StockMonitor : IStockMonitor
    {
        private readonly StockApiSettings _stockApisettings;
        private readonly HttpClient _httpClient;        

        public StockMonitor(HttpClient httpClient, IOptions<StockApiSettings> stockApiSettings) 
        {
            _httpClient = httpClient;
            _stockApisettings = stockApiSettings.Value;
        }

        public async Task<Dictionary<string, object>> QueryStockQuote(string stockName, string suffix = ".SA")
        {
            var queryParams = new List<KeyValuePair<string, string>> 
            {
                new("function", "GLOBAL_QUOTE"),
                new("symbol", stockName + suffix),
                new("apikey", _stockApisettings.ApiKey)
            };

            string completeQuery = BuildQuery(_stockApisettings.Endpoint, queryParams);
            var jsonElement = await _httpClient.GetFromJsonAsync<JsonElement>(completeQuery);
            var result = jsonElement.GetProperty("Global Quote").EnumerateObject().ToDictionary(x => x.Name, x => (object)x.Value);
            Console.WriteLine(result);
            return result;
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
            string result = baseUrl + separator + string.Join("&", queryParams.Select(kvp => kvp.Key + "=" + kvp.Value));
            return result;
        }        
    }
}
