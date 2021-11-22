using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Consumer.Domain;
using System;

namespace RabbitMQ.Infra.MessageBroker
{
    public abstract class BaseQueues
    {
        protected readonly ILogger Logger;
        protected readonly RabbitMQOptions Options;

        protected readonly IConnection Connection;
        protected readonly IModel Channel;
        protected readonly ConnectionFactory ConnectionFactory;

        protected BaseQueues(ILogger<object> logger, IOptions<RabbitMQOptions> options)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Options = options?.Value ?? throw new ArgumentNullException(nameof(options));

            try
            {
                Logger.LogInformation($"Criando conexão RabbitMQ em {DateTime.UtcNow:g}");

                ConnectionFactory = new ConnectionFactory
                {
                    UserName = Options.UserName, 
                    Password = Options.Password, 
                    VirtualHost = Options.VirtualHost, 
                    HostName = Options.HostName
                };

                Connection = ConnectionFactory.CreateConnection();
                Channel = Connection.CreateModel();
            }
            catch (Exception ex)
            {
                Logger.LogError(message: $"Erro ao inicializar conexão, message: {ex.Message}");
            }
        }
    }
}