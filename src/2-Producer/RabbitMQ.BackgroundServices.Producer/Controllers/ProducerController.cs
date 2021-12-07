using Microsoft.AspNetCore.Mvc;
using RabbitMQ.BackgroundServices.Worker.Dtos;
using RabbitMQ.Infra.MessageBroker.Interfaces;
using System.Text.Json;

namespace RabbitMQ.BackgroundServices.Producer.Controllers
{
    [ApiController]
    [Route("producer")]
    public class ProducerController : ControllerBase
    {
        private readonly ILogger<ProducerController> _logger;
        private readonly IQueueProducer _queueProducer;

        public ProducerController(ILogger<ProducerController> logger, IQueueProducer queueProducer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _queueProducer = queueProducer ?? throw new ArgumentNullException(nameof(queueProducer));
        }

        [HttpPost]
        public IActionResult Post([FromBody] Message message)
        {
            _logger.LogInformation("Recebendo mensagem para postar na fila");
            
            _queueProducer.Send(message: JsonSerializer.Serialize(message));

            _logger.LogInformation("Mensagem publicada na fila");

            return Ok(message);
        }
    }
}