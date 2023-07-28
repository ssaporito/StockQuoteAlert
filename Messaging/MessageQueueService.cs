using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using Microsoft.Extensions.Options;

namespace Messaging
{
    public class MessageQueueService : IMessageQueueService
    {
        private readonly IModel _channel;
        private readonly RabbitMQSettings _mqSettings;

        public MessageQueueService(IModel channel, IOptions<RabbitMQSettings> mqSettings)
        {
            _channel = channel;
            _mqSettings = mqSettings.Value;
        }

        public void DeclareQueue(string queue, string routingKey)
        {
            _channel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: queue, exchange: _mqSettings.ExchangeName, routingKey: routingKey);
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

        public void PublishMessage(string routingKey, string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: _mqSettings.ExchangeName, routingKey: routingKey, basicProperties: null, body: body);
        }
    }
}
