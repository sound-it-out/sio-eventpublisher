using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SIO.Domain.EventPublications.Events;
using SIO.Infrastructure.EntityFrameworkCore.DbContexts;
using SIO.Infrastructure.Projections;

namespace SIO.Domain.EventPublications.Projections.Managers
{
    internal sealed class EventPublicationFailureProjectionManager : ProjectionManager<EventPublicationFailure>
    {
        private readonly IEnumerable<IProjectionWriter<EventPublicationFailure>> _projectionWriters;
        private readonly ISIOProjectionDbContextFactory _projectionDbContextFactory;
        private readonly EventPublicationOptions _eventPublicationOptions;

        public EventPublicationFailureProjectionManager(ILogger<ProjectionManager<EventPublicationFailure>> logger,
            IEnumerable<IProjectionWriter<EventPublicationFailure>> projectionWriters) : base(logger)
        {
            if (projectionWriters == null)
                throw new ArgumentNullException(nameof(projectionWriters));

            _projectionWriters = projectionWriters;

            Handle<EventPublicationFailed>(HandleAsync);
        }

        public async Task HandleAsync(EventPublicationFailed @event, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(EventPublicationQueueProjectionManager)}.{nameof(HandleAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await Task.WhenAll(_projectionWriters.Select(pw => pw.AddAsync(@event.Id, () => new EventPublicationFailure
            {
                EventId = @event.Id,
                Error = @event.Error,
                Subject = @event.Subject
            }, cancellationToken)));
        }

        public override async Task ResetAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(EventPublicationFailureProjectionManager)}.{nameof(ResetAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await Task.WhenAll(_projectionWriters.Select(pw => pw.ResetAsync(cancellationToken)));
        }
    }
}
