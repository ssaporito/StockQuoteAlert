namespace Messaging;

public class RabbitMQSettings
{
    public string HostName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ExchangeName { get; set; } = default!;
    public string ExchangeType { get; set; } = default!;
}