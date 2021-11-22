﻿using RabbitMQ.Client.Events;
using System;

namespace RabbitMQ.Domain.Interfaces
{
    public interface IQueueFactory
    {
        void CreateQueue();

        EventingBasicConsumer CreateConsumer(EventHandler<BasicDeliverEventArgs> received);
    }
}