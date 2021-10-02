using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SIO.Domain.EventPublications.Aggregates;
using SIO.Domain.EventPublications.Commands;
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

        public PublishEventCommandHandler(ILogger<PublishEventCommandHandler> logger,
            IAggregateRepository aggregateRepository,
            IEventBusPublisher eventBusPublisher)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            if (aggregateRepository == null)
                throw new ArgumentNullException(nameof(aggregateRepository));
            if (eventBusPublisher == null)
                throw new ArgumentNullException(nameof(eventBusPublisher));

            _logger = logger;
            _aggregateRepository = aggregateRepository;
            _eventBusPublisher = eventBusPublisher;
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
                var aggregateState = aggregate.GetState();

                var notification = new EventNotification<IEvent>(streamId: aggregate.Id,
                    @event: aggregateState.Event,
                    correlationId: aggregateState.CorrelationId,
                    causationId: aggregateState.CausationId,
                    timestamp: aggregateState.Event.Timestamp,
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
