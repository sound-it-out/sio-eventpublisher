using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SIO.Domain.EventPublications.Aggregates;
using SIO.Domain.EventPublications.Commands;
using SIO.Infrastructure;
using SIO.Infrastructure.Commands;
using SIO.Infrastructure.Domain;
using SIO.Infrastructure.Events;

namespace SIO.Domain.EventPublications.CommandHandlers
{
    internal sealed class PublishEventCommandHandler : ICommandHandler<PublishEventCommand>
    {
        private readonly ILogger<PublishEventCommandHandler> _logger;
        private readonly IAggregateRepository _aggregateRepository;
        private readonly IEventBusPublisher _eventBusPublisher;
        private readonly IEventStore _eventStore;

        public PublishEventCommandHandler(ILogger<PublishEventCommandHandler> logger,
            IAggregateRepository aggregateRepository,
            IEventBusPublisher eventBusPublisher,
            IEventStore eventStore)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            if (aggregateRepository == null)
                throw new ArgumentNullException(nameof(aggregateRepository));
            if (eventBusPublisher == null)
                throw new ArgumentNullException(nameof(eventBusPublisher));
            if (eventStore == null)
                throw new ArgumentNullException(nameof(eventStore));

            _logger = logger;
            _aggregateRepository = aggregateRepository;
            _eventBusPublisher = eventBusPublisher;
            _eventStore = eventStore;
        }

        public async Task ExecuteAsync(PublishEventCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(QueueEventPublicationCommandHandler)}.{nameof(ExecuteAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            var aggregate = await _aggregateRepository.GetAsync<EventPublication, EventPublicationState>(command.Subject, cancellationToken);

            try
            {
                // Event has been published but we are ahead of projections!
                if (aggregate.GetState().Status == EventPublicationStatus.Succeeded)
                    return;

                var eventContext = await _eventStore.GetEventAsync(Subject.From(aggregate.Id), cancellationToken);

                var notification = new EventNotification<IEvent>(streamId: aggregate.Id,
                    @event: eventContext.Payload,
                    correlationId: eventContext.CorrelationId,
                    causationId: eventContext.CausationId,
                    timestamp: eventContext.Payload.Timestamp,
                    userId: command.Actor);

                await _eventBusPublisher.PublishAsync(notification, cancellationToken);

                aggregate.Succeed();
            }
            catch (Exception ex)
            {
                aggregate.Fail(ex.Message);
            }

            await _aggregateRepository.SaveAsync(aggregate, command, aggregate.Version - 1, cancellationToken);
        }
    }
}
