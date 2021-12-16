using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Consumer.Domain;
using RabbitMQ.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace RabbitMQ.Infra.MessageBroker
{
    public class QueueFactory : BaseQueues, IQueueFactory
    {
        public QueueFactory(ILogger<QueueFactory> logger, IOptions<RabbitMQOptions> options) : base(logger, options)
        {
        }

        public void CreateQueues()
        {
            WaitQueue();
            ConsumerQueue();
            CreateDeadLetterQueue();
        }

        private void WaitQueue()
        {
            /*Direct: encaminha mensagens que possuam exatamente a mesma rota das filas associadas.
            /Exemplo: Uma fila se associa a um exchange com a rota foo. Quando uma nova mensagem com a rota foo chega no direct exchange ele a encaminha para a fila foo.*/
            Channel.ExchangeDeclare(Options.WaitExchangeQueue, ExchangeType.Direct, durable: true);

            Channel.QueueDeclare(Options.WaitQueue, durable: true, exclusive: false, autoDelete: false, arguments: new Dictionary<string, object>
            {
                ["x-dead-letter-exchange"] = Options.ConsumingExchangeQueue,
                ["x-dead-letter-routing-key"] = Options.ConsumingQueue,
                ["x-message-ttl"] = Options.TimeToLiveInMilisseconds
            });

            Channel.QueueBind(queue: Options.WaitQueue, exchange: Options.WaitExchangeQueue, routingKey: Options.WaitQueue);
        }

        private void ConsumerQueue()
        {
            Channel.ExchangeDeclare(Options.ConsumingExchangeQueue, ExchangeType.Direct, durable: true);

            Channel.QueueDeclare(Options.ConsumingQueue, durable: true, exclusive: false, autoDelete: false, arguments: new Dictionary<string, object>
            {
                ["x-dead-letter-exchange"] = Options.DeadLetterExchangeQueue,
                ["x-dead-letter-routing-key"] = Options.DeadLetterQueue
            });

            Channel.QueueBind(queue: Options.ConsumingQueue, exchange: Options.ConsumingExchangeQueue, routingKey: Options.ConsumingQueue);
        }

        private void CreateDeadLetterQueue()
        {
            /*Fanout:  Seu comportamento resume-se em mandar uma cópia das mensagens para todas as filas que estão associadas a ele.*/
            Channel.ExchangeDeclare(Options.DeadLetterExchangeQueue, ExchangeType.Fanout, durable: true, autoDelete: false);

            Channel.QueueDeclare(Options.DeadLetterQueue, durable: true, exclusive: false, autoDelete: false);

            Channel.QueueBind(queue: Options.DeadLetterQueue, exchange: Options.DeadLetterExchangeQueue, routingKey: Options.DeadLetterQueue);
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