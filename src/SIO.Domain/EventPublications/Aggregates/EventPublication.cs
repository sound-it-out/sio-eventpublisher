using SIO.Domain.EventPublications.Events;
using SIO.Infrastructure;
using SIO.Infrastructure.Domain;
using SIO.Infrastructure.Events;

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

        public void Queue(StreamId streamId,
            CorrelationId? correlationId,
            CausationId? causationId,
            IEvent @event,
            string type,
            string subject)
        {
            Apply(new EventPublicationQueued(
                streamId: streamId,
                correlationId: correlationId,
                causationId: causationId,
                @event: @event,
                type: type,
                subject: subject,
                version: Version++
            ));
        }

        public void Fail(string error)
        {
            Apply(new EventPublicationFailed(
                error: error,
                subject: _state.Subject,
                version: Version++
            ));
        }

        public void Succeed()
        {
            Apply(new EventPublicationSucceded(
                subject: _state.Subject,
                version: Version++
            ));
        }

        private void Handle(EventPublicationQueued @event)
        {
            Id = @event.StreamId;
            _state.Subject = Subject.From(@event.Subject);
            _state.CorrelationId = @event.CorrelationId;
            _state.CausationId = @event.CausationId;
            _state.Event = @event.Event;
            _state.Type = @event.Type;
            _state.Attempts = 0;
            _state.Status = EventPublicationStatus.Queued;
        }

        private void Handle(EventPublicationFailed @event)
        {
            _state.Attempts++;
            _state.Status = EventPublicationStatus.Failed;
        }

        private void Handle(EventPublicationSucceded @event)
        {
            _state.Status = EventPublicationStatus.Succeeded;
        }
    }
}
