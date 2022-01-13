using Microsoft.EntityFrameworkCore;
using SIO.EntityFrameworkCore.DbContexts;
using SIO.Infrastructure.EntityFrameworkCore.DbContexts;
using SIO.Infrastructure.EntityFrameworkCore.Migrations;

namespace SIO.Migrations
{
    public static class MigratorExtensions
    {
        public static Migrator AddContexts(this Migrator migrator) 
            => migrator.WithDbContext<SIOProjectionDbContext>(o => o.UseSqlServer("Server=.,1450;Initial Catalog=sio-eventpublisher-projections;User Id=sa;Password=1qaz-pl,", b => b.MigrationsAssembly("SIO.Migrations")))
                .WithDbContext<SIOStoreDbContext>(o => o.UseSqlServer("Server=.,1450;Initial Catalog=sio-store;User Id=sa;Password=1qaz-pl,", b => b.MigrationsAssembly("SIO.Migrations")))
                .WithDbContext<SIOEventPublisherStoreDbContext>(o => o.UseSqlServer("Server=.,1450;Initial Catalog=sio-event-eventpublisher-store;User Id=sa;Password=1qaz-pl,", b => b.MigrationsAssembly("SIO.Migrations")));
    }
}
