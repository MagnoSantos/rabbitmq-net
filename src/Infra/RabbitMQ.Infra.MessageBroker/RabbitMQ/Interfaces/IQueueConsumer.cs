namespace RabbitMQ.Infra.MessageBroker.Interfaces
{
    public interface IQueueConsumer
    {
        void Subscribe<TMessage>();
    }
}