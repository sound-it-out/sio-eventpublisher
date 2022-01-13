using System;
using Microsoft.Extensions.DependencyInjection;
using SIO.Domain.EventPublications;
using SIO.Domain.EventPublications.CommandHandlers;
using SIO.Domain.EventPublications.Commands;
using SIO.Domain.EventPublications.Projections;
using SIO.Domain.EventPublications.Projections.Managers;
using SIO.Domain.EventPublications.Services;
using SIO.Infrastructure.Commands;
using SIO.Infrastructure.EntityFrameworkCore.Projections;
using SIO.Infrastructure.Projections;

namespace SIO.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddScoped<ICommandHandler<QueueEventPublicationCommand>, QueueEventPublicationCommandHandler>();
            services.AddScoped<ICommandHandler<PublishEventCommand>, PublishEventCommandHandler>();
            services.AddHostedService<EventProcessor>();
            services.AddHostedService<EventPublisher>();
            services.Configure<EventProcessorOptions>(o => o.Interval = 300);
            services.Configure<EventPublisherOptions>(o => o.Interval = 300);
            services.Configure<EventPublicationOptions>(o => o.MaxRetries = 5);
            return services;
        }
    }
}
