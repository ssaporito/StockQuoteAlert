using Common.Dtos.Stock;

namespace StockMonitorService.Messaging
{
    public interface IStockMonitorBroker
    {
        void PublishStockAlert(StockAlert stockAlert);
        void ConsumeMonitorRequests();
    }
}