namespace StockMonitorService
{
    public interface IStockMonitor
    {
        Task<Dictionary<string, object>> QueryStockQuote(string stockName, string suffix = ".SA");
        Task SendBuyAlert(string stockName, decimal price);
        Task SendSellAlert(string stockName, decimal price);
    }
}
