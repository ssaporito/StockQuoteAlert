namespace StockMonitorService
{
    public interface IStockMonitorService
    {
        Task<decimal> QueryStockQuote(string stockName, string suffix = ".SA");
        Task SendBuyAlert(string stockName, decimal price);
        Task SendSellAlert(string stockName, decimal price);
    }
}
