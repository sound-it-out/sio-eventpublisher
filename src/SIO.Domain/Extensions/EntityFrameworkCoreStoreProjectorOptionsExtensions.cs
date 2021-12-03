using SIO.Domain.EventPublications.Projections;
using SIO.Domain.EventPublications.Projections.Managers;
using SIO.Infrastructure.EntityFrameworkCore.Extensions;

namespace SIO.Domain.Extensions
{
    public static class EntityFrameworkCoreStoreProjectorOptionsExtensions
    {
        public static void WithDomainProjections(this EntityFrameworkCoreStoreProjectorOptions options)
            => options.WithProjection<EventPublicationFailure, EventPublicationFailureProjectionManager>(o => o.Interval = 5000)
                .WithProjection<EventPublicationQueue, EventPublicationQueueProjectionManager>(o => o.Interval = 5000);
    }
}
