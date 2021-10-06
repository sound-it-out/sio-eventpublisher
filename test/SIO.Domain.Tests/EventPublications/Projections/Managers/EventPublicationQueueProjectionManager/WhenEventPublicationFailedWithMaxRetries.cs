using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SIO.Domain.EventPublications;
using SIO.Domain.EventPublications.Events;
using SIO.Domain.EventPublications.Projections;
using SIO.Infrastructure;
using SIO.Infrastructure.Events;
using SIO.Infrastructure.Testing.Abstractions;
using SIO.Infrastructure.Testing.Attributes;
using Xunit.Abstractions;

namespace SIO.Domain.Tests.EventPublications.Projections.Managers.EventPublicationQueueProjectionManager
{
    public sealed class WhenEventPublicationFailedWithMaxRetries : ProjectionManagerSpecification<SIO.Domain.EventPublications.Projections.Managers.EventPublicationQueueProjectionManager, EventPublicationQueue>
    {
        private readonly Subject _subject = Subject.New();
        private readonly DateTimeOffset _publicationDate = DateTimeOffset.UtcNow;
        private readonly string _error = "error";

        public WhenEventPublicationFailedWithMaxRetries(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        protected override void BuildServices(IServiceCollection services)
        {
            services.Configure<EventPublicationOptions>(o =>
            {
                o.MaxRetries = 1;
            });
        }

        protected override IEnumerable<IEvent> Given()
        {
            yield return new EventPublicationQueued(_subject, 1, _publicationDate);
            yield return new EventPublicationFailed(_error, _subject, 2);
            yield return new EventPublicationFailed(_error, _subject, 3);
        }

        [Then]
        public void ShouldNotHaveEventPublicationQueue()
        {
            Projection.ShouldBeNull();
        }
    }
}
