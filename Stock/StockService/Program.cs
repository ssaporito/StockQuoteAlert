using Messaging;
using StockMonitorService;

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
