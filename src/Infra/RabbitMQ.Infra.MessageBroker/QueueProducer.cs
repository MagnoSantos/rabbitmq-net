using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Infra.MessageBroker.Interfaces;
using RabbitMQ.Infra.MessageBroker.Options;
using System;
using System.Net.Mime;
using System.Text;

namespace RabbitMQ.Infra.MessageBroker
{
    public class QueueProducer : BaseQueues, IQueueProducer
    {
        private IBasicProperties _basicProperties;

        public QueueProducer(ILogger<object> logger, IOptions<RabbitMQOptions> options) : base(logger, options)
        {
        }

        public void Send(string message)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(message);
                
                _basicProperties = Channel.CreateBasicProperties();
                _basicProperties.ContentType = MediaTypeNames.Application.Json;
                _basicProperties.CorrelationId = Guid.NewGuid().ToString();
                _basicProperties.Expiration = "36000000";

                Channel.BasicPublish(exchange: Options.ConsumingExchangeQueue,
                                     routingKey: Options.ConsumingQueue, 
                                     mandatory: false,
                                     basicProperties: _basicProperties, 
                                     body: body);
            }
            catch (Exception ex)
            {
                Logger.LogError("Erro ao realizar publicação da mensagem:{message}", ex.Message);
            }
        }
    }
}