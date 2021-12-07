using Microsoft.AspNetCore.Mvc;
using RabbitMQ.BackgroundServices.Worker.Dtos;
using RabbitMQ.Infra.MessageBroker.Interfaces;
using System.Text.Json;

namespace RabbitMQ.Api.Producer.Controllers
{
    [ApiController]
    [Route("/api/producer")]
    public class ProducerController : Controller
    {
        private readonly IQueueProducer _queueProducer;
        private readonly ILogger<ProducerController> _logger;

        public ProducerController(IQueueProducer queueProducer, ILogger<ProducerController> logger)
        {
            _queueProducer = queueProducer ?? throw new ArgumentNullException(nameof(queueProducer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public IActionResult Post([FromBody] Message message)
        {
            var messageToPublish = JsonSerializer.Serialize(message);

            _logger.LogInformation("Publicando mensagem na fila");
            _queueProducer.Send(message: messageToPublish);
            _logger.LogInformation("Mensagem publicada na fila");

            return Ok(message);
        }
    }
}
