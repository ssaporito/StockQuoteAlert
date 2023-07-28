using MailService;
using MailService.MailSender;
using MailService.Messaging;
using MailService.Smtp;
using Messaging.Extensions;
using Messaging.MessageQueueService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.AddSingleton<ISmtpClient, SmtpClientWrapper>();        
        services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
        services.AddSingleton<IMailSender, MailSender>();
        services.SetUpRabbitMq(configuration);
        services.AddSingleton<IMessageQueueService, MessageQueueService>();
        services.AddSingleton<IMailBroker, MailBroker>();
        services.AddHostedService<Worker>();        
    })
    .Build();

await host.RunAsync();
