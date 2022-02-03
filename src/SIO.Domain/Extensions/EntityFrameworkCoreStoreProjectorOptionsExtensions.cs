using Microsoft.Extensions.Configuration;
using SIO.Domain.EventPublications.Projections;
using SIO.Domain.EventPublications.Projections.Managers;
using SIO.EntityFrameworkCore.DbContexts;
using SIO.Infrastructure.EntityFrameworkCore.Extensions;

namespace SIO.Domain.Extensions
{
    public static class EntityFrameworkCoreStoreProjectorOptionsExtensions
    {
        public static void WithDomainProjections(this EntityFrameworkCoreStoreProjectorOptions options, IConfiguration configuration)
            => options.WithProjection<EventPublicationFailure, EventPublicationFailureProjectionManager, SIOEventPublisherStoreDbContext>(o => o.Interval = configuration.GetValue<int>("EventPublicationFailure:Interval"))
                .WithProjection<EventPublicationQueue, EventPublicationQueueProjectionManager, SIOEventPublisherStoreDbContext>(o => o.Interval = configuration.GetValue<int>("EventPublicationQueue:Interval"));
    }
}
