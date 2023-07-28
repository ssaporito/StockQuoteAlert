using StockMonitorService;
using Moq;
using Common.Dtos.Stock;

namespace StockMonitorServiceTest
{
    public class StockMonitorTests
    {
        private Mock<IStockApiService> _mockApiService;
        private StockMonitor _stockMonitor;

        public StockMonitorTests()
        {
            _mockApiService = new Mock<IStockApiService>();
            _stockMonitor = new StockMonitor(_mockApiService.Object);
        }        

        [Fact]
        public async Task MonitorRegisteredStocks_ShouldIssueBuyAlert_WhenPriceDropsBelowBuyPrice()
        {
            // Arrange
            StockMonitorRequest stockToMonitor = new("TSLA", 600m, 700m);
            _stockMonitor.SetMonitoring(stockToMonitor);
            var stockData = new Dictionary<string, object>
            {
                { "price", 590m },
                { "change", 0m }
            };

            _mockApiService.Setup(service => service.QueryStockQuote(stockToMonitor.StockName, It.IsAny<string>()))
                .ReturnsAsync(stockData);
            
            // Act
            var alerts = _stockMonitor.MonitorRegisteredStocks();

            // Assert
            Assert.Contains(alerts, a => a.MonitorRequest == stockToMonitor && a.AlertType == StockAlertType.Buy);          
        }

        [Fact]
        public async Task MonitorRegisteredStocks_ShouldIssueSellAlert_WhenPriceRisesAboveSellPrice()
        {
            // Arrange
            StockMonitorRequest stockToMonitor = new("TSLA", 600m, 700m);
            _stockMonitor.SetMonitoring(stockToMonitor);
            var stockData = new Dictionary<string, object>
            {
                { "price", 710m },
                { "change", 0m }
            };

            _mockApiService.Setup(service => service.QueryStockQuote(stockToMonitor.StockName, It.IsAny<string>()))
                .ReturnsAsync(stockData);

            // Act
            var alerts = _stockMonitor.MonitorRegisteredStocks();

            // Assert
            Assert.Contains(alerts, a => a.MonitorRequest == stockToMonitor && a.AlertType == StockAlertType.Sell);
        }
    }
}