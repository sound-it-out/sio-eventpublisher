using System;
using SIO.Infrastructure.Events;
using SIO.Infrastructure.Projections;

namespace SIO.Domain.EventPublications.Projections
{
    public class EventPublicationQueue : IProjection
    {
        public string Subject { get; set; }
        public string StreamId {  get; set; }
        public string CorrelationId {  get; set; }
        public string CausationId { get; set; }
        public IEvent Event { get; set; }
        public string Type { get; set; }
        public int Attempts { get; set; }
        public DateTimeOffset? PublicationDate { get; set; }
    }
}
