using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging
{
    public class MessageQueueService : IMessageQueueService
    {
        private readonly IModel _channel;

        public MessageQueueService(IModel channel)
        {
            _channel = channel;
        }

        public void DeclareQueue(string queue)
        {
            _channel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void ConsumeQueue(string queue, Func<string, Task> onReceived)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await onReceived(message);
            };

            _channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
        }
    }
}
