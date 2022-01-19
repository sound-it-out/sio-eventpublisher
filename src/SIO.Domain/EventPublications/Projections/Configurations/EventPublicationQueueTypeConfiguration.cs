using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIO.Infrastructure.EntityFrameworkCore.EntityConfiguration;

namespace SIO.Domain.EventPublications.Projections.Configurations
{
    internal sealed class EventPublicationQueueTypeConfiguration : IProjectionTypeConfiguration<EventPublicationQueue>
    {
        public void Configure(EntityTypeBuilder<EventPublicationQueue> builder)
        {
            builder.ToTable(nameof(EventPublicationQueue));
            builder.HasKey(epq => epq.Subject);
            builder.Property(epq => epq.Subject)
                   .ValueGeneratedNever();
            builder.HasIndex(epf => epf.EventSubject);
        }
    }
}
