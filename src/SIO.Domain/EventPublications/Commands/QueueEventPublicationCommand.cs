using SIO.Infrastructure;
using SIO.Infrastructure.Commands;
using SIO.Infrastructure.Events;

namespace SIO.Domain.EventPublications.Commands
{
    internal class QueueEventPublicationCommand : Command
    {
        public IEventContext<IEvent> EventContext { get; }
        public QueueEventPublicationCommand(IEventContext<IEvent> eventContext,
            string subject,
            CorrelationId? correlationId,
            int version,
            Actor actor) : base(subject, correlationId, version, actor)
        {
            EventContext = eventContext;
        }
    }
}
