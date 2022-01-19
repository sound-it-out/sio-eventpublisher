using System;
using SIO.Domain.EventPublications.Events;
using SIO.Infrastructure.Domain;

namespace SIO.Domain.EventPublications.Aggregates
{
    public sealed class EventPublication : Aggregate<EventPublicationState>
    {
        public EventPublication(EventPublicationState state) : base(state)
        {
            Handles<EventPublicationQueued>(Handle);
            Handles<EventPublicationFailed>(Handle);
            Handles<EventPublicationSucceded>(Handle);
        }

        public override EventPublicationState GetState() => new EventPublicationState(_state);

        public void Queue(string subject,
            DateTimeOffset? publicationDate,
            string eventSubject)
        {
            Apply(new EventPublicationQueued(
                subject: subject,
                version: Version + 1,
                publicationDate: publicationDate,
                eventSubject: eventSubject
            ));
        }

        public void Fail(string error)
        {
            Apply(new EventPublicationFailed(
                error: error,
                subject: Id,
                version: Version + 1,
                eventSubject: _state.EventSubject
            ));
        }

        public void Succeed()
        {
            Apply(new EventPublicationSucceded(
                subject: Id,
                version: Version + 1
            ));
        }

        private void Handle(EventPublicationQueued @event)
        {
            Id = @event.Subject;
            _state.PublicationDate = @event.PublicationDate;
            _state.Attempts = 0;
            _state.Status = EventPublicationStatus.Queued;
            _state.EventSubject = @event.EventSubject;
            Version = @event.Version;
        }

        private void Handle(EventPublicationFailed @event)
        {
            _state.Attempts++;
            _state.Status = EventPublicationStatus.Failed;
            Version = @event.Version;
        }

        private void Handle(EventPublicationSucceded @event)
        {
            _state.Attempts++;
            _state.Status = EventPublicationStatus.Succeeded;
            Version = @event.Version;
        }
    }
}
