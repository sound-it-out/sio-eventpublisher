using SIO.Infrastructure.Events;

namespace SIO.Domain.EventPublications.Events
{
    public class EventPublicationFailed : Event
    {
        public string Error { get; }
        public string EventSubject { get; set; }

        public EventPublicationFailed(string error, string subject, int version, string eventSubject) : base(subject, version)
        {
            Error = error;
            EventSubject = subject;
        }
    }
}
