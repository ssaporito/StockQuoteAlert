using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Messaging
{
    public class ModelFactory : IDisposable
    {
        private readonly IConnection _connection;
        private readonly RabbitMQSettings _settings;

        public ModelFactory(IConnectionFactory connectionFactory, IOptions<RabbitMQSettings> settings)
        {
            _settings = settings.Value;
            _connection = connectionFactory.CreateConnection();
        }

        public IModel CreateChannel()
        {
            var channel = _connection.CreateModel();
            channel.ExchangeDeclare(exchange: _settings.ExchangeName, type: _settings.ExchangeType);
            return channel;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
