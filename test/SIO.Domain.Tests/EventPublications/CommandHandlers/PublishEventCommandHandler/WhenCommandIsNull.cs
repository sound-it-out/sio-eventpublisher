﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using SIO.Domain.EventPublications.Commands;
using SIO.Infrastructure.Domain;
using SIO.Infrastructure.Events;
using SIO.Infrastructure.Testing.Abstractions;
using SIO.Infrastructure.Testing.Attributes;
using Xunit.Abstractions;

namespace SIO.Domain.Tests.EventPublications.CommandHandlers.PublishEventCommandHandler
{
    public sealed class WhenCommandIsNull : CommandHandlerSpecification<PublishEventCommand>
    {
        private readonly Mock<IAggregateRepository> _mockAggregateRepository = new();
        private readonly Mock<IEventBusPublisher> _mockEventBusPublisher = new();
        private readonly Mock<IEventStore> _mockEventStore = new();

        public WhenCommandIsNull(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        protected override PublishEventCommand Given() => null;
        protected override Type Handler() => typeof(SIO.Domain.EventPublications.CommandHandlers.PublishEventCommandHandler);

        protected override Task When()
        {
            RecordExceptions();
            return Task.CompletedTask;
        }

        protected override void BuildServices(IServiceCollection services)
        {
            services.AddSingleton(_mockAggregateRepository.Object);
            services.AddSingleton(_mockEventBusPublisher.Object);
            services.AddSingleton(_mockEventStore.Object);
        }        

        [Then]
        public void ShouldHaveArgumentNullExceptionThrown()
        {
            Exception.ShouldNotBeNull(); 
            Exception.ShouldBeOfType<ArgumentNullException>();
        }

        [Then]
        public void ShouldHaltExecution()
        {
            _mockAggregateRepository.Invocations.Count.ShouldBe(0);
            _mockEventBusPublisher.Invocations.Count.ShouldBe(0);
            _mockEventStore.Invocations.Count.ShouldBe(0);
        }
    }
}
