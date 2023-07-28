﻿namespace StockMonitorService
{
    public interface IStockApiService
    {
        Task<Dictionary<string, object>> QueryStockQuote(string stockName, string suffix = ".SA");
    }
}