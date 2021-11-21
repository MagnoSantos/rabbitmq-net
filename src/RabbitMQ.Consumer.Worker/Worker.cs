using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Consumer.Domain;
using System.Text;
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
            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                UserName = _options.UserName,
                Password = _options.Password,
                VirtualHost = _options.VirtualHost
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _options.ConsumerQueue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var message = "Hello World!";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: _options.ConsumerExchangeQueue,
                                 routingKey: _options.ConsumerQueue,
                                 basicProperties: null,
                                 body: body);

            _logger.LogInformation("Enviado {0}", message);
            return Task.CompletedTask;
        }
    }
}