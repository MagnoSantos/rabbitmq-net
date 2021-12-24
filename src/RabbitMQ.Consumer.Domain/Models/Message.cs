using Newtonsoft.Json;
using System;

namespace RabbitMQ.Domain.Models
{
    public class Message : IMessage
    {
        /// <summary>
        /// Dados de objeto complexo para postagem na fila
        /// </summary>
        [JsonProperty("data")]
        public string Data { get; set; }
        public Guid CorrelationId { get; set; } 
    }

    public interface IMessage
    {
        [JsonProperty("correlationId")]
        public Guid CorrelationId { get; set; }
    }
}