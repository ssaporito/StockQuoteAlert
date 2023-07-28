namespace MailService.Messaging
{
    public interface IMailBroker
    {
        void ConsumeMailMessages();
    }
}