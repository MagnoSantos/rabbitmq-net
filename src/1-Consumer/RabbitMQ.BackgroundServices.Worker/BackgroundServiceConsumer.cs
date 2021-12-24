using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Domain.Dtos;
using RabbitMQ.Infra.MessageBroker.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ.Consumer.Worker
{
    [ExcludeFromCodeCoverage]
    public class BackgroundServiceConsumer : BackgroundService
    {
        private readonly IQueueConsumer _queueConsumer;
        private readonly IQueueProducer _queueProducer;
        private readonly ILogger<BackgroundServiceConsumer> _logger;

        public BackgroundServiceConsumer(IQueueConsumer queueConsumer, IQueueProducer queueProducer, ILogger<BackgroundServiceConsumer> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _queueProducer = queueProducer ?? throw new ArgumentNullException(nameof(queueProducer));
            _queueConsumer = queueConsumer ?? throw new ArgumentNullException(nameof(queueConsumer));
        }

        /// <summary>
        /// Exemplo 1 - Publicação e consumo de fila
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            _logger.LogInformation("Cadastrando consumidor");
            await Task.Factory.StartNew(action: () => _queueConsumer.Subscribe<Message>(), stoppingToken);
            _logger.LogInformation("Consumidor cadastrado");

            _logger.LogInformation("Publicando mensagem na fila");
            var message = new Message(new Customer
            {
                Cpf = "12567763600",
                LastName = "Santos",
                Name = "Magno"
            });
            _queueProducer.Send(message.ToJson());
            _logger.LogInformation(@"Mensagem publicada na fila, {message}: ", message.ToJson());
        }
    }
}