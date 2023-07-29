using Messaging.Extensions;
using Messaging.MessageQueueService;
using StockMonitorService;
using StockMonitorService.Messaging;
using StockMonitorService.StockApi;
using StockMonitorService.StockMonitor;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.AddSingleton<HttpClient>();
        services.Configure<StockApiSettings>(configuration.GetSection("StockApiSettings"));
        services.SetUpRabbitMq(configuration);
        services.AddSingleton<IMessageQueueService, MessageQueueService>();
        services.AddSingleton<IStockApiService, StockApiService>();
        services.AddSingleton<IStockMonitor, StockMonitor>();
        services.AddSingleton<IStockMonitorBroker, StockMonitorBroker>();
        services.AddHostedService<StockMonitorWorker>();        
    })
    .Build();

await host.RunAsync();
