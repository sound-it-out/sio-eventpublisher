using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using SIO.Domain.EventPublications.Aggregates;
using SIO.Domain.EventPublications.Events;
using SIO.Infrastructure;
using SIO.Infrastructure.Events;
using SIO.Infrastructure.Testing.Abstractions;
using SIO.Infrastructure.Testing.Attributes;

namespace SIO.Domain.Tests.EventPublications.Aggregates.EventPublication
{
    public sealed class WhenEventPublicationQueued : AggregateSpecification<SIO.Domain.EventPublications.Aggregates.EventPublication, SIO.Domain.EventPublications.Aggregates.EventPublicationState>
    {
        private readonly Subject _subject = Subject.New();
        private readonly DateTimeOffset _publicationDate = DateTimeOffset.UtcNow;
        protected override IEnumerable<IEvent> Given()
        {
            yield break;
        }

        protected override void When()
        {
            Aggregate.Queue(_subject, _publicationDate);
        }

        [Then]
        public void ShouldContainOneUncommitedEvent()
        {
            Aggregate.GetUncommittedEvents().Count().ShouldBe(1);
        }

        [Then]
        public void ShouldContainUncommitedEventPublicationQueuedEvent()
        {
            var events = Aggregate.GetUncommittedEvents();

            events.Single().ShouldBeOfType<EventPublicationQueued>();
        }

        [Then]
        public void ShouldContainUncommitedEventPublicationQueuedEventWithExpectedVersion()
        {
            var @event = Aggregate.GetUncommittedEvents()
                .OfType<EventPublicationQueued>()
                .Single();

            @event.Version.ShouldBe(1);
        }

        [Then]
        public void ShouldContainUncommitedEventPublicationQueuedEventWithExpectedPublicationDate()
        {
            var @event = Aggregate.GetUncommittedEvents()
                .OfType<EventPublicationQueued>()
                .Single();

            @event.PublicationDate.ShouldBe(_publicationDate);
        }

        [Then]
        public void ShouldContainUncommitedEventPublicationQueuedEventWithExpectedSubject()
        {
            var @event = Aggregate.GetUncommittedEvents()
                .OfType<EventPublicationQueued>()
                .Single();

            @event.Subject.ShouldBe(_subject);
        }

        [Then]
        public void ShouldHaveExpectedVersion()
        {
            Aggregate.Version.ShouldBe(1);
        }

        [Then]
        public void ShouldHaveExpectedId()
        {
            Aggregate.Id.ShouldBe(_subject);
        }

        [Then]
        public void ShouldContainStateWithExpectedAttempts()
        {
            Aggregate.GetState().Attempts.ShouldBe(0);
        }

        [Then]
        public void ShouldContainStateWithExpectedPublicationDate()
        {
            Aggregate.GetState().PublicationDate.ShouldBe(_publicationDate);
        }       

        [Then]
        public void ShouldContainStateWithExpectedStatus()
        {
            Aggregate.GetState().Status.ShouldBe(EventPublicationStatus.Queued);
        }
    }
}
