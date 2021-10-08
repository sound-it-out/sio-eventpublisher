using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Shouldly;
using SIO.Domain.EventPublications.Events;
using SIO.Domain.EventPublications.Projections;
using SIO.Infrastructure;
using SIO.Infrastructure.Events;
using SIO.Infrastructure.Projections;
using SIO.Infrastructure.Testing.Abstractions;
using SIO.Infrastructure.Testing.Attributes;
using Xunit.Abstractions;

namespace SIO.Domain.Tests.EventPublications.Projections.Managers.EventPublicationFailureProjectionManager
{
    public sealed class WhenEventPublicationFailedAndCancellationIsRequested : ProjectionManagerSpecification<EventPublicationFailure>
    {
        private readonly Mock<IProjectionWriter<EventPublicationFailure>> _mockProjectionWriter = new();
        private readonly Subject _subject = Subject.New();
        private readonly string _error = "error";

        public WhenEventPublicationFailedAndCancellationIsRequested(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        protected override sealed Type ProjectionManager() => typeof(SIO.Domain.EventPublications.Projections.Managers.EventPublicationFailureProjectionManager);
        protected override sealed void When() 
        {
            RecordExceptions();
            _cancellationTokenSource.Cancel();
        }


        protected override sealed IEnumerable<IEvent> Given()
        {
            yield return new EventPublicationFailed(_error, _subject, 2);
        }        

        protected override sealed void BuildServices(IServiceCollection services)
        {
            services.RemoveAll<IProjectionWriter<EventPublicationFailure>>();
            services.AddSingleton(_mockProjectionWriter.Object);
        }

        [Then]
        public void ShouldHaveNullProjection()
        {
            Projection.ShouldBeNull();
        }

        [Then]
        public void ShouldHaveOperationCanceledExceptionThrown()
        {
            Exception.ShouldNotBeNull();
            Exception.ShouldBeOfType<OperationCanceledException>();
        }

        [Then]
        public void ShouldHaltExecution()
        {
            _mockProjectionWriter.Invocations.Count.ShouldBe(0);
        }
    }
}
