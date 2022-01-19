using System;
using SIO.Infrastructure;
using SIO.Infrastructure.Commands;

namespace SIO.Domain.EventPublications.Commands
{
    internal class QueueEventPublicationCommand : Command
    {
        public DateTimeOffset? PublicationDate { get; }
        public string EventSubject { get; }
        public QueueEventPublicationCommand(string subject,
            CorrelationId? correlationId,
            int version,
            Actor actor,
            DateTimeOffset? publicationDate,
            string eventSubject) : base(subject, correlationId, version, actor)
        {
            PublicationDate = publicationDate;
            EventSubject = eventSubject;
        }
    }
}
