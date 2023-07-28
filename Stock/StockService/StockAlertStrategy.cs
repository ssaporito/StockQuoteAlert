using Common.Dtos.Stock;

namespace StockMonitorService
{
    public static class StockAlertStrategy
    {
        public static bool ShouldSell(StockMonitorRequest request, decimal? previousPrice, decimal currentPrice)
        {
            return currentPrice > request.SellPrice && (!previousPrice.HasValue || previousPrice.Value <= request.SellPrice);
        }

        public static bool ShouldBuy(StockMonitorRequest request, decimal? previousPrice, decimal currentPrice)
        {
            return currentPrice < request.BuyPrice && (!previousPrice.HasValue || previousPrice.Value >= request.BuyPrice);
        }

        public static StockAlertType? BuyOrSell(StockMonitorRequest request, decimal? previousPrice, decimal currentPrice)
        {
            if (ShouldSell(request, previousPrice, currentPrice)) return StockAlertType.Sell;
            if (ShouldBuy(request, previousPrice, currentPrice)) return StockAlertType.Buy;
            return null;
        }
    }
}
