using Common.Dtos.Stock;

namespace StockMonitorService.StockMonitor
{
    public interface IStockMonitor
    {
        IEnumerable<StockAlert> MonitorRegisteredStocks();
        void SetMonitoring(StockMonitorRequest stockMonitorRequest);
        void RemoveMonitoring(StockMonitorRequest stockMonitorRequest);
        StockMonitorData? GetStockQuote(StockMonitorRequest stockMonitorRequest);
    }
}
