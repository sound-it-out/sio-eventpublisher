using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIO.Infrastructure.EntityFrameworkCore.EntityConfiguration;

namespace SIO.Domain.EventPublications.Projections.Configurations
{
    internal sealed class EventPublicationFailureTypeConfiguration : IProjectionTypeConfiguration<EventPublicationFailure>
    {
        public void Configure(EntityTypeBuilder<EventPublicationFailure> builder)
        {
            builder.ToTable(nameof(EventPublicationFailure));
            builder.HasKey(epf => epf.Subject);
            builder.Property(epf => epf.Subject)
                   .ValueGeneratedNever();
            builder.HasIndex(epf => epf.EventId);
        }
    }
}
