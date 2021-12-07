﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Infra.MessageBroker.Interfaces;
using RabbitMQ.Infra.MessageBroker.Options;
using System;
using System.Text;
using System.Text.Json;

namespace RabbitMQ.Infra.MessageBroker
{
    public class QueueConsumer : BaseQueues, IQueueConsumer
    {
        private readonly IQueueFactory _queueFactory;

        public QueueConsumer(ILogger<QueueConsumer> logger, IOptions<RabbitMQOptions> options, IQueueFactory queueFactory) : base(logger, options)
        {
            _queueFactory = queueFactory ?? throw new ArgumentNullException(nameof(queueFactory));
        }

        public void Subscribe<TMessage>()
        {
            _queueFactory.CreateQueue();

            var consumer = _queueFactory.CreateConsumer((ch, ea) =>
            {
                try
                {
                    TMessage data = default;

                    string message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    data = JsonSerializer.Deserialize<TMessage>(message);

                    Channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Erro ao desserializar mensagem", ex.Message);

                    //TODO: Adicionar fila de DLQ
                    Channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
                }
            });

            Channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            Channel.BasicConsume(Options.ConsumerQueue, autoAck: false, string.Empty, false, false, null, consumer);
        }
    }
}