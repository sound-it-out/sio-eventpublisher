using System;
using SIO.Infrastructure.Domain;

namespace SIO.Domain.EventPublications.Aggregates
{
    public sealed class EventPublicationState : IAggregateState
    {
        public int Attempts { get; set; }
        public EventPublicationStatus Status {  get; set; }
        public DateTimeOffset? PublicationDate { get; set; }

        public EventPublicationState() { }
        public EventPublicationState(EventPublicationState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            Attempts = state.Attempts;
            Status = state.Status;
            PublicationDate = state.PublicationDate;
        }
    }
}
