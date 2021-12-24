using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Infra.MessageBroker.Options;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RabbitMQ.Infra.MessageBroker
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseQueues : IDisposable
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
                Logger.LogError("Erro ao inicializar conexão {message}", ex.Message);
            }
        }

        public void Dispose()
        {
            Logger.LogInformation("Fechando conexão com o RabbitMQ");
            Channel.Close();
            Channel.Dispose();
            Connection.Close();
            Connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}