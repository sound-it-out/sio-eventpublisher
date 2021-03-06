using System;
using System.Linq;
using System.Runtime.CompilerServices;
using SIO.Domain.EventPublications.Events;
[assembly: InternalsVisibleTo("SIO.Domain.Tests")]
namespace SIO.Domain
{
    public static class EventHelper
    {
        public static Type[] AllEvents = new IntegrationEvents.AllEvents().Concat(new Type[]
        {
            typeof(EventPublicationQueued),
            typeof(EventPublicationFailed),
            typeof(EventPublicationSucceded)
        }).ToArray();
    }
}
