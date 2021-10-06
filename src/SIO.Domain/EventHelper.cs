using System;
using System.Linq;
using System.Runtime.CompilerServices;
using SIO.Domain.Documents.Events;
using SIO.Domain.EventPublications.Events;
using SIO.Domain.Translations.Events;
using SIO.Domain.Users.Events;
[assembly: InternalsVisibleTo("SIO.Domain.Tests")]
namespace SIO.Domain
{
    public static class EventHelper
    {
        public static Type[] PublicationEvents = new Type[]
        {
            typeof(DocumentDeleted),
            typeof(DocumentUploaded),
            typeof(TranslationCharactersProcessed),
            typeof(TranslationFailed),
            typeof(TranslationQueued),
            typeof(TranslationStarted),
            typeof(TranslationSucceded),
            typeof(TranslationSynthesiseQueued),
            typeof(TranslationSynthesiseFailed),
            typeof(UserEmailChanged),
            typeof(UserLoggedIn),
            typeof(UserLoggedOut),
            typeof(UserPasswordTokenGenerated),
            typeof(UserPurchasedCharacterTokens),
            typeof(UserRegistered),
            typeof(UserVerified)
        };

        public static Type[] AllEvents = PublicationEvents.Concat(new Type[]
        {
            typeof(EventPublicationQueued),
            typeof(EventPublicationFailed),
            typeof(EventPublicationSucceded)
        }).ToArray();
    }
}
