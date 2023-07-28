using Common.Dtos.Mail;
using Common.Helpers;
using MailService.MailSender;
using Messaging.MessageQueueService;
using System.Text.Json;

namespace MailService.Messaging
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
