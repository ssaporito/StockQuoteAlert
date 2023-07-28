using MailService;
using Messaging;

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
        services.AddHostedService<MailWorker>();        
    })
    .Build();

await host.RunAsync();
