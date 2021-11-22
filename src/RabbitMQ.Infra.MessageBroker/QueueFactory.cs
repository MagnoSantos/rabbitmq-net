using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Consumer.Domain;
using RabbitMQ.Domain.Interfaces;
using System;

namespace RabbitMQ.Infra.MessageBroker
{
    public class QueueFactory : BaseQueues, IQueueFactory
    {
        public QueueFactory(ILogger<QueueFactory> logger, IOptions<RabbitMQOptions> options) : base(logger, options)
        {
        }

        public void CreateQueue()
        {
            Channel.ExchangeDeclare(Options.ConsumerExchangeQueue, ExchangeType.Direct, durable: true);
            Channel.QueueDeclare(Options.ConsumerQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        public EventingBasicConsumer CreateConsumer(EventHandler<BasicDeliverEventArgs> received)
        {
            var consumer = new EventingBasicConsumer(Channel);

            consumer.Shutdown += (_, args) => Logger.LogInformation(message: $"O consumifor foi encerrado.");
            consumer.Registered += (_, args) => Logger.LogInformation(message: $"Um novo consumidor foi registrado");
            consumer.Unregistered += (_, args) => Logger.LogInformation(message: $"Um consumidor existente não foi registrao.");
            consumer.ConsumerCancelled += (_, args) => Logger.LogInformation(message: $"Um consumidor existente foi cancelado.");
            consumer.Received += received;

            return consumer;
        }
    }
}