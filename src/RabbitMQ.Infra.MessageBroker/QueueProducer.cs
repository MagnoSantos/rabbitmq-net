using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Consumer.Domain;
using RabbitMQ.Domain.Interfaces;
using System;
using System.Text;

namespace RabbitMQ.Infra.MessageBroker
{
    public class QueueProducer : BaseQueues, IQueueProducer
    {
        public QueueProducer(ILogger<object> logger, IOptions<RabbitMQOptions> options) : base(logger, options)
        {
        }

        public void Send(string message)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(message);

                Channel.BasicPublish(exchange: Options.ConsumerExchangeQueue,
                                     routingKey: Options.ConsumerQueue, 
                                     mandatory: false,
                                     basicProperties: null, 
                                     body: body);
            }
            catch (Exception ex)
            {
                Logger.LogError("Erro ao realizar publicação da mensagem:{message}", ex.Message);
            }
        }
    }
}