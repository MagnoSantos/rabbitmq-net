using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Consumer.Domain;
using RabbitMQ.Domain.Interfaces;
using RabbitMQ.Infra.MessageBroker;

namespace RabbitMQ.Infra.CrossCutting.IoC
{
    public static class IoC
    {
        public static IServiceCollection ConfigureContainer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMQOptions>(configuration.GetSection(RabbitMQOptions.RabbitConfig));

            services.AddScoped<IQueueFactory, QueueFactory>()
                    .AddScoped<IQueueConsumer, QueueConsumer>()
                    .AddScoped<IQueueProducer, QueueProducer>();

            return services;
        }
    }
}