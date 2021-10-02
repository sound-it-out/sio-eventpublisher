using SIO.Infrastructure.Events;

namespace SIO.Domain.EventPublications.Events
{
    public class EventPublicationSucceded : Event
    {
        public EventPublicationSucceded(string subject, int version) : base(subject, version)
        {
        }
    }
}
