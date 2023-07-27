namespace Messaging
{
    public interface IMessageQueueService
    {
        void DeclareQueue(string queue);
        void ConsumeQueue(string queue, Func<string, Task> onReceived);        
    }
}
