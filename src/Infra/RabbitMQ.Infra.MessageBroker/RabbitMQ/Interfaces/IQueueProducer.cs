namespace RabbitMQ.Infra.MessageBroker.Interfaces
{
    public interface IQueueProducer
    {
        void Send(string message);
    }
}