using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Consumer.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ.Consumer.Worker
{
    public class Worker : BackgroundService
    {
        private readonly RabbitMQOptions _options;
        private readonly ILogger<Worker> _logger;

        public Worker(IOptions<RabbitMQOptions> options,
                      ILogger<Worker> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}