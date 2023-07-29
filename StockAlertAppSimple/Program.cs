using Common.Dtos.Mail;
using MailService.MailSender;
using MailService.Smtp;
using Messaging.MessageQueueService;
using StockAlertAppSimple;
using StockMonitorService.Messaging;
using StockMonitorService.StockApi;
using StockMonitorService.StockMonitor;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.Configure<MailInfo>(configuration.GetSection("MailInfo"));
        services.AddSingleton<ISmtpClient, SmtpClientWrapper>();
        services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
        services.AddSingleton<IMailSender, MailSender>();

        services.AddSingleton<HttpClient>();
        services.Configure<StockApiSettings>(configuration.GetSection("StockApiSettings"));                
        services.AddSingleton<IStockApiService, StockApiService>();
        services.AddSingleton<IStockMonitor, StockMonitor>();        

        services.AddHostedService<StockAlertAppSimpleWorker>();
    })
    .Build();

await host.RunAsync();
