using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Messaging.RabbitMq
{
    public class ModelFactory : IDisposable
    {
        private readonly IConnection _connection;
        private readonly RabbitMQSettings _settings;
        private int _retryCount = 5;

        public ModelFactory(IConnectionFactory connectionFactory, IOptions<RabbitMQSettings> settings)
        {
            _settings = settings.Value;
            _connection = TryConnect(connectionFactory);
        }

        private IConnection TryConnect(IConnectionFactory connectionFactory)
        {
            while (_retryCount > 0)
            {

                try
                {
                    var connection = connectionFactory.CreateConnection();
                    return connection;
                }
                catch (BrokerUnreachableException)
                {
                    _retryCount--;
                    Thread.Sleep(5000);
                    continue;
                }                
            }

            throw new Exception("Unable to connect to RabbitMQ.");
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
