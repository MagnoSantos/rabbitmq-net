using System;
using System.Text.Json.Serialization;

namespace RabbitMQ.BackgroundServices.Worker.Dtos
{
    public class Message : IMessage
    {
        /// <summary>
        /// Dados de objeto complexo para postagem na fila
        /// </summary>
        [JsonPropertyName("data")]
        public string Data { get; set; }
        public Guid CorrelationId { get; } = Guid.NewGuid();
    }

    public interface IMessage
    {
        [JsonPropertyName("correlationId")]
        public Guid CorrelationId { get; }
    }
}