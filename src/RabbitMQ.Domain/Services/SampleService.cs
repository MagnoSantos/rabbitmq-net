using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Domain.Dtos;

namespace RabbitMQ.Domain.Services
{
    public class SampleService : ISampleService
    {
        private readonly ILogger<SampleService> _logger;

        public SampleService(ILogger<SampleService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Execute(Message message, IBasicProperties basicProperties)
        {
            if (basicProperties is null)
            {
                throw new ArgumentNullException(nameof(basicProperties));
            }
            else if (message is null or { Data: null })
            {
                throw new ArgumentNullException(nameof(message));
            }

            message.IncludeBasicProperties(
                correlationId: Guid.Parse(basicProperties.CorrelationId),
                contentType: basicProperties.ContentType,
                expiration: basicProperties.Expiration
            );

            _logger.LogInformation(@"Mensagem com propriedades adicionais pelo header: {message}, propriedades: {0}, {1}, {2}", message.ToJson(),
                basicProperties.CorrelationId, basicProperties.ContentType, basicProperties.Expiration
            );
        }
    }
}