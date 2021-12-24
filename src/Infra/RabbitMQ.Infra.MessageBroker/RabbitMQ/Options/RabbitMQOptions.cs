namespace RabbitMQ.Infra.MessageBroker.Options
{
    public class RabbitMQOptions
    {
        public const string RabbitConfig = "RabbitConfig";

        public string UserName { get; init; }
        public string Password { get; init; }
        public string HostName { get; init; }
        public string VirtualHost { get; init; }
        public int Port { get; init; }

        public string ConsumingQueue { get; init; }
        public string ConsumingExchangeQueue { get; init; }
        public string WaitQueue { get; init; }
        public string WaitExchangeQueue { get; init; }
        public string DeadLetterExchangeQueue { get; init; }
        public string DeadLetterQueue { get; init; }
        public int TimeToLiveInMilisseconds { get; init; }
    }
}