using SIO.Infrastructure.Projections;

namespace SIO.Domain.EventPublications.Projections
{
    public class EventPublicationFailure : IProjection
    {
        public string Subject { get; set; }
        public string EventSubject { get; set; }
        public string Error { get; set; }
    }
}
