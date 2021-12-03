using Microsoft.Extensions.Configuration;
using SIO.Infrastructure;
using SIO.Infrastructure.Azure.ServiceBus.Extensions;
using SIO.Infrastructure.RabbitMQ.Extensions;

namespace SIO.EventPublisher.Extensions
{
    internal static class SIOInfrastructureBuilderExtensions
    {
        public static ISIOInfrastructureBuilder AddEventBus(this ISIOInfrastructureBuilder builder, IConfiguration configuration)
        {
            var azureServiceBusConnection = configuration.GetConnectionString("AzureServiceBus");
            var rabbitMqConnection = configuration.GetConnectionString("RabbitMq");

            if (!string.IsNullOrWhiteSpace(azureServiceBusConnection))
            {
                builder.AddAzureServiceBus(options =>
                {
                    options.UseConnection(azureServiceBusConnection)
                    .UseTopic(e =>
                    {
                        e.WithName(configuration.GetValue<string>("Azure:ServiceBus:Topic"));
                    });
                });
            }
            else if (!string.IsNullOrWhiteSpace(rabbitMqConnection))
            {
                var managementApiEndpoint = configuration.GetValue<string>("RabbitMq:ManagementApi:Endpoint");
                builder.AddRabbitMq(options =>
                {
                    options.UseConnection(rabbitMqConnection)
                    .UseExchange(o =>
                    {
                        o.WithName(configuration.GetValue<string>("RabbitMq:Exchange"));
                        o.UseTopicExchangeType();
                    });

                    if (!string.IsNullOrWhiteSpace(managementApiEndpoint))
                    {
                        options.UseManagementApi(o =>
                        {
                            o.WithEndpoint(managementApiEndpoint);
                            o.WithCredentials(
                                user: configuration.GetValue<string>("RabbitMq:ManagementApi:User"),
                                password: configuration.GetValue<string>("RabbitMq:ManagementApi:Password")
                            );
                        });
                    }
                });
            }

            return builder;
        }
    }
}
