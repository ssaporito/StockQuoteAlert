using System.Text.Json;
using Messaging;

namespace MailService
{
    public class MailWorker : BackgroundService
    {
        private readonly ILogger<MailWorker> _logger;
        private readonly IMailSender _mailSender;
        private readonly IMessageQueueService _mqService;
        private readonly string _queueName = "mail";

        public MailWorker(ILogger<MailWorker> logger, IMailSender mailSender, IMessageQueueService messageQueueService)
        {
            _logger = logger;
            _mailSender = mailSender;
            _mqService = messageQueueService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {            
            _mqService.DeclareQueue(_queueName, _queueName);
            _mqService.ConsumeQueue(_queueName, async message =>
            {
                Console.WriteLine("Received {0}", message);
                var email = JsonSerializer.Deserialize<EmailMessage>(message);
                if (email != null)
                {
                    await _mailSender.SendMail(email);
                    _logger.LogInformation($"E-mail sent to {email.RecipientEmail}");
                }
                else
                {
                    _logger.LogWarning($"E-mail was not sent due to bad format.");
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