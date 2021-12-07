﻿namespace RabbitMQ.Infra.MessageBroker.Options
{
    public class RabbitMQOptions
    {
        public const string RabbitConfig = "RabbitConfig";

        #region Creditials

        public string UserName { get; init; }
        public string Password { get; init; }
        public string HostName { get; init; }
        public string VirtualHost { get; init; }
        public int Port { get; init; }

        #endregion Creditials

        public string ConsumerQueue { get; init; }
        public string ConsumerExchangeQueue { get; init; }

        public int NumberOfRetriesToProcessMessage { get; init; }
    }
}