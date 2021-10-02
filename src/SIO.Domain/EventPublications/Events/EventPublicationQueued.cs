using SIO.Infrastructure;
using SIO.Infrastructure.Events;

namespace SIO.Domain.EventPublications.Events
{
    public class EventPublicationQueued : Event
    {
        public StreamId StreamId { get; set; }
        public CorrelationId? CorrelationId { get; set; }
        public CausationId? CausationId { get; set; }
        public IEvent Event { get; set; }
        public string Type { get; set; }

        public EventPublicationQueued(StreamId streamId,
            CorrelationId? correlationId,
            CausationId? causationId,
            IEvent @event,
            string type,
            string subject,
            int version) : base(subject, version)
        {
            StreamId = streamId;
            CorrelationId = correlationId;
            CausationId = causationId;
            Event = @event;
            Type = type;
        }
    }
}
