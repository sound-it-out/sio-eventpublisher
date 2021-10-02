using Microsoft.Extensions.DependencyInjection;
using SIO.Domain.EventPublications.Projections;
using SIO.Domain.EventPublications.Projections.Managers;
using SIO.Domain.EventPublications.Services;
using SIO.Infrastructure.Projections;

namespace SIO.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddScoped<IProjectionManager<EventPublicationFailure>, EventPublicationFailureProjectionManager>();
            services.AddScoped<IProjectionManager<EventPublicationQueue>, EventPublicationQueueProjectionManager>();
            services.AddHostedService<EventProcessor>();
            services.AddHostedService<EventPublisher>();
            services.Configure<EventProcessorOptions>(o => o.Interval = 300);
            services.Configure<EventPublisherOptions>(o => o.Interval = 300);
            return services;
        }
    }
}
