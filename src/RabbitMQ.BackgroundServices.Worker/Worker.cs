using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Domain.Interfaces;
using RabbitMQ.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ.Consumer.Worker
{
    public class Worker : BackgroundService
    {
        private readonly IQueueConsumer _queueConsumer;
        private readonly ILogger<Worker> _logger;

        public Worker(IQueueConsumer queueConsumer, ILogger<Worker> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _queueConsumer = queueConsumer ?? throw new ArgumentNullException(nameof(queueConsumer));
        }

        /// <summary>
        /// Exemplo - Postagem na fila com exchange e TTL
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            _logger.LogInformation("Cadastrando consumidor");
            ///Objeto fábrica para criar e configurar instâncias Task - Task.StartNew cria e inicia uma nova tarefa em uma única chamada
            await Task.Factory.StartNew(action: () => _queueConsumer.Subscribe<Message>(), stoppingToken);
            _logger.LogInformation("Consumidor cadastrado");
        }
    }
}