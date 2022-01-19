using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SIO.Domain.EventPublications.Aggregates;
using SIO.Domain.EventPublications.Commands;
using SIO.EntityFrameworkCore.DbContexts;
using SIO.Infrastructure.Commands;
using SIO.Infrastructure.Domain;
using SIO.Infrastructure.Events;

namespace SIO.Domain.EventPublications.CommandHandlers
{
    internal sealed class QueueEventPublicationCommandHandler : ICommandHandler<QueueEventPublicationCommand>
    {
        private readonly ILogger<QueueEventPublicationCommandHandler> _logger;
        private readonly IAggregateRepository<SIOEventPublisherStoreDbContext> _aggregateRepository;
        private readonly IAggregateFactory _aggregateFactory;

        public QueueEventPublicationCommandHandler(ILogger<QueueEventPublicationCommandHandler> logger,
            IAggregateRepository<SIOEventPublisherStoreDbContext> aggregateRepository,
            IAggregateFactory aggregateFactory)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            if (aggregateRepository == null)
                throw new ArgumentNullException(nameof(aggregateRepository));
            if (aggregateFactory == null)
                throw new ArgumentNullException(nameof(aggregateFactory));

            _logger = logger;
            _aggregateRepository = aggregateRepository;
            _aggregateFactory = aggregateFactory;
        }

        public async Task ExecuteAsync(QueueEventPublicationCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(QueueEventPublicationCommandHandler)}.{nameof(ExecuteAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            var aggregate = await _aggregateRepository.GetAsync<EventPublication, EventPublicationState>(command.Subject, cancellationToken);

            if (aggregate != null)
                return;

            aggregate = _aggregateFactory.FromHistory<EventPublication, EventPublicationState>(Enumerable.Empty<IEvent>());

            if (aggregate == null)
                throw new ArgumentNullException(nameof(aggregate));

            aggregate.Queue(
                subject: command.Subject,
                publicationDate: command.PublicationDate,
                eventSubject: command.EventSubject
            );

            await _aggregateRepository.SaveAsync(aggregate, command, cancellationToken: cancellationToken);
        }
    }
}
