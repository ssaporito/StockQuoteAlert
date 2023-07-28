namespace Common.Dtos.Stock
{    
    public record StockMonitorRequest(string StockName, decimal BuyPrice, decimal SellPrice);
    public record StockMonitorData(decimal Price, decimal Change);
    public record StockAlert(StockMonitorRequest MonitorRequest, StockMonitorData MonitorData, StockAlertType AlertType);
    public enum StockAlertType { Buy, Sell };
}