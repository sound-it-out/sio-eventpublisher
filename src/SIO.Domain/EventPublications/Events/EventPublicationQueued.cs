using System;
using SIO.Infrastructure.Events;

namespace SIO.Domain.EventPublications.Events
{
    public class EventPublicationQueued : Event
    {
        public DateTimeOffset? PublicationDate { get; }

        public EventPublicationQueued(string subject, int version, DateTimeOffset? publicationDate) : base(subject, version)
        {
            PublicationDate = publicationDate;
        }
    }
}
