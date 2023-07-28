using Common.Dtos.Mail;
using Common.Helpers;
using Messaging;
using System.Text.Json;

namespace MailService
{
    public class MailBroker : IMailBroker
    {
        private readonly IMailSender _mailSender;
        private readonly IMessageQueueService _mqService;
        private readonly string _queueName = QueueNames.SendMailQueue;

        public MailBroker(IMailSender mailSender, IMessageQueueService mqService)
        {
            _mailSender = mailSender;
            _mqService = mqService;
        }

        public void ConsumeMailMessages()
        {
            _mqService.DeclareQueue(_queueName, _queueName);
            _mqService.ConsumeQueue(_queueName, async message =>
            {
                Console.WriteLine("Received {0}", message);
                var email = JsonSerializer.Deserialize<EmailMessage>(message);
                if (email != null)
                {
                    await _mailSender.SendMail(email);
                }
            });
        }
    }
}
