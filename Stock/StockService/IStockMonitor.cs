using Common.Dtos.Stock;

namespace StockMonitorService
{
    public interface IStockMonitor
    {
        List<StockAlert> MonitorRegisteredStocks();
        void SetMonitoring(StockMonitorRequest stockMonitorRequest);
        void RemoveMonitoring(StockMonitorRequest stockMonitorRequest);
        StockMonitorData? GetStockQuote(StockMonitorRequest stockMonitorRequest);        
    }
}
