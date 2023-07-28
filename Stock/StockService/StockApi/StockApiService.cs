using Common.Helpers.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace StockMonitorService.StockApi
{
    public class StockApiService : IStockApiService
    {
        private readonly HttpClient _httpClient;
        private readonly StockApiSettings _stockApisettings;

        public StockApiService(HttpClient httpClient, IOptions<StockApiSettings> stockApiSettings)
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

            string completeQuery = HttpHelpers.BuildQuery(_stockApisettings.Endpoint, queryParams);
            var jsonElement = await _httpClient.GetFromJsonAsync<JsonElement>(completeQuery);
            var result = jsonElement.GetProperty("Global Quote").EnumerateObject().ToDictionary(x => x.Name, x => (object)x.Value);
            return result;
        }
    }
}
