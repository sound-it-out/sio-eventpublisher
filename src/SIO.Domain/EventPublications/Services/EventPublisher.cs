using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SIO.Domain.EventPublications.Commands;
using SIO.Domain.EventPublications.Projections;
using SIO.Infrastructure;
using SIO.Infrastructure.Commands;
using SIO.Infrastructure.EntityFrameworkCore.DbContexts;

namespace SIO.Domain.EventPublications.Services
{
    internal sealed class EventPublisher : IHostedService
    {
        private Task _executingTask;
        private CancellationTokenSource StoppingCts { get; set; }
        private readonly IServiceScope _scope;
        private readonly ILogger<EventPublisher> _logger;
        private readonly IOptionsMonitor<EventPublisherOptions> _options;
        private readonly ISIOProjectionDbContextFactory _projectionDbContextFactory;
        private readonly string _name;
        private readonly ICommandDispatcher _commandDispatcher;

        public EventPublisher(IServiceScopeFactory serviceScopeFactory,
            IOptionsMonitor<EventPublisherOptions> options,
            ILogger<EventPublisher> logger)
        {
            if (serviceScopeFactory == null)
                throw new ArgumentNullException(nameof(serviceScopeFactory));
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _scope = serviceScopeFactory.CreateScope();
            _logger = logger;
            _options = options;
            _projectionDbContextFactory = _scope.ServiceProvider.GetRequiredService<ISIOProjectionDbContextFactory>();
            _commandDispatcher = _scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();

            _name = typeof(EventPublisher).FullName;
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(EventPublisher)}.{nameof(StartAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            _logger.LogInformation($"{nameof(EventPublisher)} starting");
            StoppingCts = new();

            _executingTask = ExecuteAsync(StoppingCts.Token);

            _logger.LogInformation($"{nameof(EventPublisher)} started");

            if (_executingTask.IsCompleted)
                return _executingTask;

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(EventPublisher)}.{nameof(StopAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            _logger.LogInformation($"{nameof(EventPublisher)} stopping");

            if (_executingTask == null)
                return;

            try
            {
                StoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
                _logger.LogInformation($"{nameof(EventPublisher)} stopped");
            }
        }

        private async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(EventPublisher)}.{nameof(ExecuteAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using (var context = _projectionDbContextFactory.Create())
                    {
                        var eventsInQueue = await context.Set<EventPublicationQueue>()
                            .AsQueryable()
                            .Where(epq => !epq.PublicationDate.HasValue || epq.PublicationDate <= DateTimeOffset.UtcNow)
                            .Take(50)
                            .Select(epq => epq.Subject)
                            .ToArrayAsync();

                        var correlationId = CorrelationId.New();

                        foreach (var @event in eventsInQueue)
                        {
                            await _commandDispatcher.DispatchAsync(new PublishEventCommand(
                                subject: @event,
                                correlationId: correlationId,
                                version: 0,
                                Actor.Unknown
                            ));
                        }

                        if (eventsInQueue.Count() == 0)
                            await Task.Delay(_options.CurrentValue.Interval);
                        else
                            await context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, $"Process '{typeof(EventPublisher).Name}' failed due to an unexpected error. See exception details for more information.");
                    break;
                }
            }
        }
    }
}
