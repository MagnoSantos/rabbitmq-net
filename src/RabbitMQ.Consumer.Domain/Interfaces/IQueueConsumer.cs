namespace RabbitMQ.Domain.Interfaces
{
    public interface IQueueConsumer
    {
        void Subscribe<TMessage>();
    }
}