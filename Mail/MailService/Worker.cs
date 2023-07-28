using MailService.Messaging;

namespace MailService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMailBroker _mailBroker;

        public Worker(ILogger<Worker> logger, IMailBroker mailBroker)
        {
            _logger = logger;
            _mailBroker = mailBroker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {  
            while (!stoppingToken.IsCancellationRequested)
            {
                _mailBroker.ConsumeMailMessages();
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }                     
    }    
}