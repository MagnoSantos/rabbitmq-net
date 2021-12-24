using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Domain.Services;
using RabbitMQ.Infra.MessageBroker;
using RabbitMQ.Infra.MessageBroker.Interfaces;
using RabbitMQ.Infra.MessageBroker.Options;

namespace RabbitMQ.Infra.CrossCutting.IoC
{
    public static class IoC
    {
        public static IServiceCollection ConfigureContainer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMQOptions>(configuration.GetSection(RabbitMQOptions.RabbitConfig));

            services.AddScoped<ISampleService, SampleService>()
                    .AddScoped<IQueueFactory, QueueFactory>()
                    .AddScoped<IQueueConsumer, QueueConsumer>()
                    .AddScoped<IQueueProducer, QueueProducer>();

            return services;
        }
    }
}