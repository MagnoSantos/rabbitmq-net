namespace RabbitMQ.Domain.Interfaces
{
    public interface IQueueProducer
    {
        void Send(string message);
    }
}