using RabbitMQ.Client.Events;
using System;

namespace RabbitMQ.Infra.MessageBroker.Interfaces
{
    public interface IQueueFactory
    {
        void CreateQueues();

        EventingBasicConsumer CreateConsumer(EventHandler<BasicDeliverEventArgs> received);
    }
}