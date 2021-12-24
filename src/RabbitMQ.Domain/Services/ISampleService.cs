using RabbitMQ.Client;
using RabbitMQ.Domain.Dtos;

namespace RabbitMQ.Domain.Services
{
    public interface ISampleService
    {
        void Execute(Message message, IBasicProperties basicProperties);
    }
}