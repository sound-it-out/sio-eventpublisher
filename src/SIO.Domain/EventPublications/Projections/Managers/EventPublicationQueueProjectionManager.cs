using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SIO.Domain.EventPublications.Events;
using SIO.Domain.EventPublications.Services;
using SIO.Infrastructure.EntityFrameworkCore.DbContexts;
using SIO.Infrastructure.Projections;

namespace SIO.Domain.EventPublications.Projections.Managers
{
    public sealed class EventPublicationQueueProjectionManager : ProjectionManager<EventPublicationQueue>
    {
        private readonly IEnumerable<IProjectionWriter<EventPublicationQueue>> _projectionWriters;
        private readonly ISIOProjectionDbContextFactory _projectionDbContextFactory;
        private readonly IOptionsMonitor<EventPublisherOptions> _options;

        public EventPublicationQueueProjectionManager(ILogger<ProjectionManager<EventPublicationQueue>> logger,
            IEnumerable<IProjectionWriter<EventPublicationQueue>> projectionWriters,
            ISIOProjectionDbContextFactory projectionDbContextFactory,
            IOptionsMonitor<EventPublisherOptions> options) : base(logger)
        {
            if (projectionWriters == null)
                throw new ArgumentNullException(nameof(projectionWriters));
            if (projectionDbContextFactory == null)
                throw new ArgumentNullException(nameof(projectionDbContextFactory));
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _projectionWriters = projectionWriters;
            _projectionDbContextFactory = projectionDbContextFactory;
            _options = options;

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

            await Task.WhenAll(_projectionWriters.Select(pw => pw.AddAsync(@event.Subject, () => new EventPublicationQueue
            {
                Attempts = 0,
                Subject = @event.Subject,
                EventSubject = @event.EventSubject,
                PublicationDate = @event.PublicationDate
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
                var eventPublication = await context.Set<EventPublicationQueue>().FindAsync(@event.Subject);
                if (eventPublication.Attempts == _options.CurrentValue.MaxRetries)
                {
                    await Task.WhenAll(_projectionWriters.Select(pw => pw.RemoveAsync(@event.Subject)));
                }
                else
                {
                    await Task.WhenAll(_projectionWriters.Select(pw => pw.UpdateAsync(@event.Subject, epq =>
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

            await Task.WhenAll(_projectionWriters.Select(pw => pw.RemoveAsync(@event.Subject)));
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
