using Messaging;
using StockMonitorService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.AddSingleton(() => new HttpClient());
        services.Configure<StockApiSettings>(configuration.GetSection("StockApiSettings"));
        services.SetUpRabbitMq(configuration);
        services.AddHostedService<StockMonitorWorker>();        
    })
    .Build();

await host.RunAsync();
