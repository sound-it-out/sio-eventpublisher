using System;
using System.Collections.Generic;
using Shouldly;
using SIO.Domain.EventPublications.Events;
using SIO.Domain.EventPublications.Projections;
using SIO.Infrastructure;
using SIO.Infrastructure.Events;
using SIO.Infrastructure.Testing.Abstractions;
using SIO.Infrastructure.Testing.Attributes;
using Xunit.Abstractions;

namespace SIO.Domain.Tests.EventPublications.Projections.Managers.EventPublicationFailureProjectionManager
{
    public sealed class WhenEventPublicationFailed : ProjectionManagerSpecification<EventPublicationFailure>
    {
        private readonly Subject _subject = Subject.New();
        private readonly Subject _eventSubject = Subject.New();
        private readonly string _error = "error";

        public WhenEventPublicationFailed(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        protected override Type ProjectionManager() => typeof(SIO.Domain.EventPublications.Projections.Managers.EventPublicationFailureProjectionManager);

        protected override IEnumerable<IEvent> Given()
        {
            yield return new EventPublicationFailed(_error, _subject, 2, _eventSubject);
        }

        [Then]
        public void ShouldHaveEventPublicationQueueWithExpectedError()
        {
            Projection.Error.ShouldBe(_error);
        }

        [Then]
        public void ShouldHaveEventPublicationQueueWithExpectedSubject()
        {
            Projection.Subject.ShouldBe(_subject);
        }        
    }
}
