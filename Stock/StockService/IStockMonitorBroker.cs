using Common.Dtos.Stock;

namespace StockMonitorService
{
    public interface IStockMonitorBroker
    {
        void AlertStockQuote(StockAlert stockAlert);
        void CheckMonitorRequests();
    }
}