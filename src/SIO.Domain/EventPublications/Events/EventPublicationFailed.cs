using SIO.Infrastructure.Events;

namespace SIO.Domain.EventPublications.Events
{
    public class EventPublicationFailed : Event
    {
        public string Error { get; }

        public EventPublicationFailed(string error, string subject, int version) : base(subject, version)
        {
            Error = error;
        }
    }
}
