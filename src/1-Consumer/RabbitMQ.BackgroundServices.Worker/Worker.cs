using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.BackgroundServices.Worker.Dtos;
using RabbitMQ.Infra.MessageBroker.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ.Consumer.Worker
{
    [ExcludeFromCodeCoverage]
    public class Worker : BackgroundService
    {
        private readonly IQueueConsumer _queueConsumer;
        private readonly ILogger<Worker> _logger;

        public Worker(IQueueFactory queueFactory, ILogger<Worker> logger, IQueueConsumer queueConsumer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _queueConsumer = queueConsumer ?? throw new ArgumentNullException(nameof(queueConsumer));
        }

        /// <summary>
        /// Exemplo 1 - Consumo de mensagem de uma fila
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            _logger.LogInformation("Cadastrando consumidor");
            await Task.Factory.StartNew(action: () => _queueConsumer.Subscribe<Message>(), stoppingToken);
            _logger.LogInformation("Consumidor cadastrado");
        }
    }
}