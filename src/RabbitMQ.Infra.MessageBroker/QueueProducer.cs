using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Consumer.Domain;
using RabbitMQ.Domain.Interfaces;

namespace RabbitMQ.Infra.MessageBroker
{
    public class QueueProducer : BaseQueues, IQueueProducer
    {
        public QueueProducer(ILogger<object> logger, IOptions<RabbitMQOptions> options) : base(logger, options)
        {
        }
    }
}