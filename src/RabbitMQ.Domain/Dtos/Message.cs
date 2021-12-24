using System.Text.Json;
using System.Text.Json.Serialization;

namespace RabbitMQ.Domain.Dtos
{
    public class Message : BaseMessage
    {
        public Message(Customer data)
        {
            Data = data;
        }

        public Customer Data { get; set; }

        public string ToJson() => JsonSerializer.Serialize(this);
    }

    public class Customer
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }

        [JsonPropertyName("cpf")]
        public string? Cpf { get; set; }
    }

    public class BaseMessage
    {
        [JsonIgnore]
        public Guid CorrelationId { get; set; }

        [JsonIgnore]
        public string? ContentType { get; set; }

        [JsonIgnore]
        public string? Expiration { get; set; }

        public void IncludeBasicProperties(Guid correlationId, string? contentType = null, string? expiration = null)
        {
            CorrelationId = correlationId;
            ContentType = contentType;
            Expiration = expiration;
        }
    }
}