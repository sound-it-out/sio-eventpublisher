using System;
using SIO.Infrastructure.Projections;

namespace SIO.Domain.EventPublications.Projections
{
    public class EventPublicationQueue : IProjection
    {
        public string Subject { get; set; }
        public int Attempts { get; set; }
        public DateTimeOffset? PublicationDate { get; set; }
    }
}
