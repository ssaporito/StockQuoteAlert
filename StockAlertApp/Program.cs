using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Messaging;
using StockAlertApp;

await Host.CreateDefaultBuilder()
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.SetUpRabbitMq(configuration);
        services.AddHostedService<StockAlertService>();
    })
    .RunConsoleAsync();