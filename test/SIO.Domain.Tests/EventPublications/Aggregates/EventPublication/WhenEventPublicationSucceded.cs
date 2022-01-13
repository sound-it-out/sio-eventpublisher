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
    public sealed class WhenEventPublicationSucceded : AggregateSpecification<SIO.Domain.EventPublications.Aggregates.EventPublication, SIO.Domain.EventPublications.Aggregates.EventPublicationState>
    {
        private readonly Subject _subject = Subject.New();
        private readonly Subject _eventSubject = Subject.New();
        private readonly DateTimeOffset _publicationDate = DateTimeOffset.UtcNow;
        private readonly string _error = "error";
        protected override IEnumerable<IEvent> Given()
        {
            yield return new EventPublicationQueued(_subject, 1, _publicationDate, _eventSubject);
            yield return new EventPublicationFailed(_error, _subject, 2, _eventSubject);
        }

        protected override void When()
        {
            Aggregate.Succeed();
        }

        [Then]
        public void ShouldContainOneUncommitedEvent()
        {
            Aggregate.GetUncommittedEvents().Count().ShouldBe(1);
        }

        [Then]
        public void ShouldContainUncommitedEventPublicationSuccededEvent()
        {
            var events = Aggregate.GetUncommittedEvents();

            events.Single().ShouldBeOfType<EventPublicationSucceded>();
        }

        [Then]
        public void ShouldContainUncommitedEventPublicationSuccededEventWithExpectedVersion()
        {
            var @event = Aggregate.GetUncommittedEvents()
                .OfType<EventPublicationSucceded>()
                .Single();

            @event.Version.ShouldBe(3);
        }

        [Then]
        public void ShouldContainUncommitedEventPublicationSuccededEventWithExpectedSubject()
        {
            var @event = Aggregate.GetUncommittedEvents()
                .OfType<EventPublicationSucceded>()
                .Single();

            @event.Subject.ShouldBe(_subject);
        }

        [Then]
        public void ShouldHaveExpectedVersion()
        {
            Aggregate.Version.ShouldBe(3);
        }

        [Then]
        public void ShouldHaveExpectedId()
        {
            Aggregate.Id.ShouldBe(_subject);
        }

        [Then]
        public void ShouldContainStateWithExpectedAttempts()
        {
            Aggregate.GetState().Attempts.ShouldBe(2);
        }

        [Then]
        public void ShouldContainStateWithExpectedPublicationDate()
        {
            Aggregate.GetState().PublicationDate.ShouldBe(_publicationDate);
        }       

        [Then]
        public void ShouldContainStateWithExpectedStatus()
        {
            Aggregate.GetState().Status.ShouldBe(EventPublicationStatus.Succeeded);
        }
    }
}
