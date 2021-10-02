using SIO.Infrastructure;
using SIO.Infrastructure.Commands;

namespace SIO.Domain.EventPublications.Commands
{
    public sealed class PublishEventCommand : Command
    {
        public PublishEventCommand(string subject, CorrelationId? correlationId, int version, Actor actor) : base(subject, correlationId, version, actor)
        {
        }
    }
}
