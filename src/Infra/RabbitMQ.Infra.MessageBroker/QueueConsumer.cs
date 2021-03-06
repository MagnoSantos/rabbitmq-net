using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Domain.Dtos;
using RabbitMQ.Domain.Services;
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
        private readonly ISampleService _sampleService;

        public QueueConsumer(ILogger<QueueConsumer> logger, IOptions<RabbitMQOptions> options, IQueueFactory queueFactory, ISampleService sampleService) 
            : base(logger, options)
        {
            _queueFactory = queueFactory ?? throw new ArgumentNullException(nameof(queueFactory));
            _sampleService = sampleService ?? throw new ArgumentNullException(nameof(sampleService));
        }

        public void Subscribe<TMessage>()
        {
            _queueFactory.CreateQueues();

            var consumer = _queueFactory.CreateConsumer((ch, ea) =>
            {
                try
                {
                    TMessage data = default;

                    string message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    data = JsonSerializer.Deserialize<TMessage>(message);

                    _sampleService.Execute(data as Message, ea.BasicProperties);

                    Channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Erro ao desserializar mensagem", ex.Message);

                    Channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
                }
            });

            Channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            Channel.BasicConsume(Options.ConsumingQueue, autoAck: false, string.Empty, false, false, null, consumer);
        }
    }
}