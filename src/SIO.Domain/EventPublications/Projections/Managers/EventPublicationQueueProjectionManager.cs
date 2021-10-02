using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SIO.Domain.EventPublications.Events;
using SIO.Infrastructure.EntityFrameworkCore.DbContexts;
using SIO.Infrastructure.Events;
using SIO.Infrastructure.Projections;

namespace SIO.Domain.EventPublications.Projections.Managers
{
    internal sealed class EventPublicationQueueProjectionManager : ProjectionManager<EventPublicationQueue>
    {
        private readonly IEnumerable<IProjectionWriter<EventPublicationQueue>> _projectionWriters;
        private readonly ISIOProjectionDbContextFactory _projectionDbContextFactory;
        private readonly EventPublicationOptions _eventPublicationOptions;

        public EventPublicationQueueProjectionManager(ILogger<ProjectionManager<EventPublicationQueue>> logger,
            IEnumerable<IProjectionWriter<EventPublicationQueue>> projectionWriters,
            ISIOProjectionDbContextFactory projectionDbContextFactory,
            IOptionsSnapshot<EventPublicationOptions> optionsSnapshot) : base(logger)
        {
            if (projectionWriters == null)
                throw new ArgumentNullException(nameof(projectionWriters));
            if (projectionDbContextFactory == null)
                throw new ArgumentNullException(nameof(projectionDbContextFactory));
            if (optionsSnapshot == null)
                throw new ArgumentNullException(nameof(optionsSnapshot));

            _projectionWriters = projectionWriters;
            _projectionDbContextFactory = projectionDbContextFactory;
            _eventPublicationOptions = optionsSnapshot.Value;

            Handle<EventPublicationQueued>(HandleAsync);
            Handle<EventPublicationFailed>(HandleAsync);
            Handle<EventPublicationSucceded>(HandleAsync);
        }

        public async Task HandleAsync(EventPublicationQueued @event, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(EventPublicationQueueProjectionManager)}.{nameof(HandleAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await Task.WhenAll(_projectionWriters.Select(pw => pw.AddAsync(@event.Event.Id, () => new EventPublicationQueue
            {
                Attempts = 0,
                CausationId = @event.CausationId,
                CorrelationId = @event.CorrelationId,
                Event = @event.Event,
                StreamId = @event.StreamId,
                Subject = @event.Event.Id,
                Type = @event.Type
            }, cancellationToken)));
        }

        public async Task HandleAsync(EventPublicationFailed @event, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(EventPublicationQueueProjectionManager)}.{nameof(HandleAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            using (var context = _projectionDbContextFactory.Create())
            {
                var email = await context.Set<EventPublicationQueue>().FindAsync(@event.Id);
                if (email.Attempts == _eventPublicationOptions.MaxRetries)
                {
                    await Task.WhenAll(_projectionWriters.Select(pw => pw.RemoveAsync(@event.Id)));
                }
                else
                {
                    await Task.WhenAll(_projectionWriters.Select(pw => pw.UpdateAsync(@event.Id, epq =>
                    {
                        epq.Attempts++;
                    })));
                }
            }
        }

        public async Task HandleAsync(EventPublicationSucceded @event, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(EventPublicationQueueProjectionManager)}.{nameof(HandleAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await Task.WhenAll(_projectionWriters.Select(pw => pw.RemoveAsync(@event.Id)));
        }

        public override async Task ResetAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(EventPublicationQueueProjectionManager)}.{nameof(ResetAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await Task.WhenAll(_projectionWriters.Select(pw => pw.ResetAsync(cancellationToken)));
        }
    }
}
