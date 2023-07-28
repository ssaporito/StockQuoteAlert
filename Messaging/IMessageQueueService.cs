namespace Messaging
{
    public interface IMessageQueueService
    {
        void DeclareQueue(string queue, string routingKey);
        void ConsumeQueue(string queue, Func<string, Task> onReceived);
        void PublishMessage(string routingKey, string message);
    }
}
