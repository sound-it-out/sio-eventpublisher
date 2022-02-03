namespace SIO.Domain.EventPublications.Services
{
    public class EventPublisherOptions
    {
        public int Interval { get; set; }
        public int MaxRetries { get; set; }
    }
}
