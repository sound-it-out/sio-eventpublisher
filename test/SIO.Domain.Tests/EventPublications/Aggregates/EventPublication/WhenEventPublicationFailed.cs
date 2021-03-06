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
    public sealed class WhenEventPublicationFailed : AggregateSpecification<SIO.Domain.EventPublications.Aggregates.EventPublication, SIO.Domain.EventPublications.Aggregates.EventPublicationState>
    {
        private readonly Subject _subject = Subject.New();
        private readonly Subject _eventSubject = Subject.New();
        private readonly DateTimeOffset _publicationDate = DateTimeOffset.UtcNow;
        private readonly string _error = "error";
        protected override IEnumerable<IEvent> Given()
        {
            yield return new EventPublicationQueued(_subject, 1, _publicationDate, _eventSubject);
        }

        protected override void When()
        {
            Aggregate.Fail(_error);
        }

        [Then]
        public void ShouldContainOneUncommitedEvent()
        {
            Aggregate.GetUncommittedEvents().Count().ShouldBe(1);
        }

        [Then]
        public void ShouldContainUncommitedEventPublicationFailedEvent()
        {
            var events = Aggregate.GetUncommittedEvents();

            events.Single().ShouldBeOfType<EventPublicationFailed>();
        }

        [Then]
        public void ShouldContainUncommitedEventPublicationFailedEventWithExpectedVersion()
        {
            var @event = Aggregate.GetUncommittedEvents()
                .OfType<EventPublicationFailed>()
                .Single();

            @event.Version.ShouldBe(2);
        }

        [Then]
        public void ShouldContainUncommitedEventPublicationFailedEventWithExpectedError()
        {
            var @event = Aggregate.GetUncommittedEvents()
                .OfType<EventPublicationFailed>()
                .Single();

            @event.Error.ShouldBe(_error);
        }

        [Then]
        public void ShouldContainUncommitedEventPublicationFailedEventWithExpectedSubject()
        {
            var @event = Aggregate.GetUncommittedEvents()
                .OfType<EventPublicationFailed>()
                .Single();

            @event.Subject.ShouldBe(_subject);
        }

        [Then]
        public void ShouldHaveExpectedVersion()
        {
            Aggregate.Version.ShouldBe(2);
        }

        [Then]
        public void ShouldHaveExpectedId()
        {
            Aggregate.Id.ShouldBe(_subject);
        }

        [Then]
        public void ShouldContainStateWithExpectedAttempts()
        {
            Aggregate.GetState().Attempts.ShouldBe(1);
        }

        [Then]
        public void ShouldContainStateWithExpectedPublicationDate()
        {
            Aggregate.GetState().PublicationDate.ShouldBe(_publicationDate);
        }       

        [Then]
        public void ShouldContainStateWithExpectedStatus()
        {
            Aggregate.GetState().Status.ShouldBe(EventPublicationStatus.Failed);
        }
    }
}
