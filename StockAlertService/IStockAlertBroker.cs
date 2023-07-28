using Common.Dtos.Mail;
using Common.Dtos.Stock;

namespace StockAlertService
{
    public interface IStockAlertBroker
    {
        void ConsumeAlerts();
        void DeclareQueues();
        void PublishMailRequest(EmailMessage alertEmail);
        void PublishMonitorRequest(StockMonitorRequest monitorRequest);
    }
}