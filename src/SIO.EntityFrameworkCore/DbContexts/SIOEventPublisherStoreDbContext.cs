using Microsoft.EntityFrameworkCore;
using SIO.Infrastructure.EntityFrameworkCore.DbContexts;

namespace SIO.EntityFrameworkCore.DbContexts
{
    public class SIOEventPublisherStoreDbContext : SIOStoreDbContextBase<SIOEventPublisherStoreDbContext>
    {
        public SIOEventPublisherStoreDbContext(DbContextOptions<SIOEventPublisherStoreDbContext> options) : base(options)
        {
        }
    }
}
