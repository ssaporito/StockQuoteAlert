using Common.Dtos.Stock;

namespace StockMonitorService
{
    public interface IStockMonitorBroker
    {
        void PublishStockAlert(StockAlert stockAlert);
        void ConsumeMonitorRequests();
    }
}