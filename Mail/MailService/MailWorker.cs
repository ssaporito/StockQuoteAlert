using System.Text.Json;
using Messaging;

namespace MailService
{
    public class MailWorker : BackgroundService
    {
        private readonly ILogger<MailWorker> _logger;
        private readonly IMailSender _mailSender;
        private readonly IMessageQueueService _mqService;

        public MailWorker(ILogger<MailWorker> logger, IMailSender mailSender, IMessageQueueService messageQueueService)
        {
            _logger = logger;
            _mailSender = mailSender;
            _mqService = messageQueueService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {            
            _mqService.DeclareQueue("email_queue");
            _mqService.ConsumeQueue("email_queue", async message =>
            {
                Console.WriteLine("Received {0}", message);
                var email = JsonSerializer.Deserialize<EmailMessage>(message);
                if (email != null)
                {
                    await _mailSender.SendMail(email);
                }
            });

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }                     
    }    
}