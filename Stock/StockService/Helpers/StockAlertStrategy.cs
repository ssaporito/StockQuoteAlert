using Common.Dtos.Stock;

namespace StockMonitorService.Helpers
{
    public static class StockAlertStrategy
    {
        public static bool ShouldSell(StockMonitorRequest request, decimal? previousPrice, decimal currentPrice)
        {
            // Garante que a venda só seja recomendada cada vez que o preço transiciona de baixo para cima do limite
            return currentPrice > request.SellPrice && (!previousPrice.HasValue || previousPrice.Value <= request.SellPrice);
        }

        public static bool ShouldBuy(StockMonitorRequest request, decimal? previousPrice, decimal currentPrice)
        {
            // Garante que a compra só seja recomendada cada vez que o preço transiciona de cima para baixo do limite
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
