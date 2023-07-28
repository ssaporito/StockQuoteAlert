namespace MailService
{
    public class MailWorker : BackgroundService
    {
        private readonly ILogger<MailWorker> _logger;
        private readonly IMailBroker _mailBroker;

        public MailWorker(ILogger<MailWorker> logger, IMailBroker mailBroker)
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