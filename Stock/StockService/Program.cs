using Messaging;
using StockService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.SetUpRabbitMq(configuration);
        services.AddHostedService<Worker>();        
    })
    .Build();

await host.RunAsync();
