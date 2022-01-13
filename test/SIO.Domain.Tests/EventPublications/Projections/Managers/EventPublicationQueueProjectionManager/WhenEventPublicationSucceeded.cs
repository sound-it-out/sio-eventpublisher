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
    public sealed class WhenEventPublicationSucceeded : ProjectionManagerSpecification<EventPublicationQueue>
    {
        private readonly Subject _subject = Subject.New();
        private readonly Subject _eventSubject = Subject.New();
        private readonly DateTimeOffset _publicationDate = DateTimeOffset.UtcNow;

        public WhenEventPublicationSucceeded(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        protected override Type ProjectionManager() => typeof(SIO.Domain.EventPublications.Projections.Managers.EventPublicationQueueProjectionManager);

        protected override void BuildServices(IServiceCollection services)
        {
            services.Configure<EventPublicationOptions>(o =>
            {
                o.MaxRetries = 0;
            });
        }

        protected override IEnumerable<IEvent> Given()
        {
            yield return new EventPublicationQueued(_subject, 1, _publicationDate, _eventSubject);
            yield return new EventPublicationSucceded(_subject, 2);
        }

        [Then]
        public void ShouldNotHaveEventPublicationQueue()
        {
            Projection.ShouldBeNull();
        }
    }
}
