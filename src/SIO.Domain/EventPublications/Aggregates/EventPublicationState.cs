using System;
using SIO.Infrastructure;
using SIO.Infrastructure.Domain;
using SIO.Infrastructure.Events;

namespace SIO.Domain.EventPublications.Aggregates
{
    public sealed class EventPublicationState : IAggregateState
    {
        public Subject Subject { get; set; }
        public CorrelationId? CorrelationId { get; set; }
        public CausationId? CausationId { get; set; }
        public IEvent Event { get; set; }
        public string Type { get; set; }
        public int Attempts { get; set; }
        public EventPublicationStatus Status {  get; set; }

        public EventPublicationState() { }
        public EventPublicationState(EventPublicationState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            Subject = state.Subject;
            CorrelationId = state.CorrelationId;
            CausationId = state.CausationId;
            Event = state.Event;
            Type = state.Type;
            Attempts = state.Attempts;
            Status = state.Status;
        }
    }
}
