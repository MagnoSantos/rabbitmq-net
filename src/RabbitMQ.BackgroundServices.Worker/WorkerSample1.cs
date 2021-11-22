using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Domain.Interfaces;
using RabbitMQ.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ.Consumer.Worker
{
    public class WorkerSample1 : BackgroundService
    {
        private readonly IQueueFactory _queueFactory;
        private readonly IQueueConsumer _queueConsumer;
        private readonly ILogger<WorkerSample1> _logger;

        public WorkerSample1(IQueueFactory queueFactory, ILogger<WorkerSample1> logger, IQueueConsumer queueConsumer)
        {
            _queueFactory = queueFactory ?? throw new ArgumentNullException(nameof(queueFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _queueConsumer = queueConsumer ?? throw new ArgumentNullException(nameof(queueConsumer));
        }

        /// <summary>
        /// Exemplo 1 - Postagem e consumo de mensagem de uma fila
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            _logger.LogInformation("Criando filas");
            _queueFactory.CreateQueue();
            _logger.LogInformation("Filas criadas");

            _logger.LogInformation("Cadastrando consumidor");
            await Task.Factory.StartNew(action: () => _queueConsumer.Subscribe<Message>(), stoppingToken);
            _logger.LogInformation("Consumidor cadastrado");
        }
    }
}