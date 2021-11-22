using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Consumer.Domain;
using RabbitMQ.Domain.Interfaces;

namespace RabbitMQ.Infra.MessageBroker
{
    public class QueueConsumer : BaseQueues, IQueueConsumer
    {
        public QueueConsumer(ILogger<object> logger, IOptions<RabbitMQOptions> options) : base(logger, options)
        {
        }
    }
}