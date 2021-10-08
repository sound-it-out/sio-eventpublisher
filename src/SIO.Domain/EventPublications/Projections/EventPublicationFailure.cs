using SIO.Infrastructure.Projections;

namespace SIO.Domain.EventPublications.Projections
{
    public class EventPublicationFailure : IProjection
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string Error { get; set; }
    }
}
