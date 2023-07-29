using StockAlertService;
using Messaging.MessageQueueService;
using Messaging.Extensions;
using StockAlertService.Messaging;
using Common.Dtos.Mail;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.Configure<MailInfo>(configuration.GetSection("MailInfo"));
        services.SetUpRabbitMq(configuration);
        services.AddSingleton<IMessageQueueService, MessageQueueService>();
        services.AddSingleton<IStockAlertBroker, StockAlertBroker>();
        services.AddHostedService<StockAlertWorker>();
    })
    .Build();

await host.RunAsync();