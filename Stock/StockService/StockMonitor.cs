using Common.Dtos.Stock;

namespace StockMonitorService
{
    public class StockMonitor : IStockMonitor
    {
        private readonly IStockApiService _stockApiService;
        private readonly List<StockMonitorRequest> _stocksToMonitor = new();
        private readonly Dictionary<StockMonitorRequest, StockMonitorData> _stockQuotes = new();        

        public StockMonitor(IStockApiService stockApiService) 
        {
            _stockApiService = stockApiService;
        }

        public List<StockAlert> MonitorRegisteredStocks()
        {          
            List<StockAlert> stocksToAlert = new();

            Parallel.ForEach(_stocksToMonitor, async monitorRequest =>
            {
                string stockName = monitorRequest.StockName;
                var rawData = await _stockApiService.QueryStockQuote(stockName);
                var monitorData = ParseStockMonitorData(rawData);
                decimal? previousPrice = _stockQuotes.ContainsKey(monitorRequest) ? _stockQuotes[monitorRequest].Price : null;
                var alertType = StockAlertStrategy.BuyOrSell(monitorRequest, previousPrice, monitorData.Price);
                if (alertType != null)
                {
                    StockAlert alert = new(monitorRequest, monitorData, alertType.Value);
                    stocksToAlert.Add(alert);
                }
                
                _stockQuotes[monitorRequest] = monitorData;
            });

            return stocksToAlert;
        }

        public StockMonitorData? GetStockQuote(StockMonitorRequest stockMonitorRequest)
        {
            return _stockQuotes.ContainsKey(stockMonitorRequest) ? _stockQuotes[stockMonitorRequest] : null;
        }

        public void RemoveMonitoring(StockMonitorRequest stockMonitorRequest)
        {            
            _stocksToMonitor.Remove(stockMonitorRequest);
        }

        public void SetMonitoring(StockMonitorRequest stockMonitorRequest)
        {
            var monitoredStock = _stocksToMonitor.FirstOrDefault(s => s == stockMonitorRequest);
            if (monitoredStock == null)
            {
                _stocksToMonitor.Add(stockMonitorRequest);
            }
        }    
        
        private StockMonitorData? ParseStockMonitorData(Dictionary<string, object> stockData)
        {
            try
            {
                string priceKey = stockData.Keys.First(k => k.Contains("price"));
                string changeKey = stockData.Keys.First(k => k.Contains("change"));

                decimal currentPrice = (decimal)stockData[priceKey];
                decimal change = (decimal)stockData[changeKey];

                StockMonitorData monitorData = new(currentPrice, change);
                return monitorData;
            }
            catch {
                return null;
            }
        }
    }
}
