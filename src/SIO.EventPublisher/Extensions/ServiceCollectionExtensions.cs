using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIO.Domain;
using SIO.Domain.Extensions;
using SIO.EntityFrameworkCore.DbContexts;
using SIO.Infrastructure.EntityFrameworkCore.DbContexts;
using SIO.Infrastructure.EntityFrameworkCore.Extensions;
using SIO.Infrastructure.EntityFrameworkCore.SqlServer.Extensions;
using SIO.Infrastructure.Extensions;
using SIO.Infrastructure.Serialization.Json.Extensions;
using SIO.Infrastructure.Serialization.MessagePack.Extensions;

namespace SIO.EventPublisher.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSIOInfrastructure()
                .AddEntityFrameworkCoreSqlServer(options =>
                {
                    options.AddStore<SIOStoreDbContext>(configuration.GetConnectionString("Store"), o => o.MigrationsAssembly($"{nameof(SIO)}.{nameof(Migrations)}"));
                    options.AddStore<SIOEventPublisherStoreDbContext>(configuration.GetConnectionString("EventPublisherStore"), o => o.MigrationsAssembly($"{nameof(SIO)}.{nameof(Migrations)}"));
                    options.AddProjections(configuration.GetConnectionString("Projection"), o => o.MigrationsAssembly($"{nameof(SIO)}.{nameof(Migrations)}"));
                })
                .AddEntityFrameworkCoreStoreProjector(options => options.WithDomainProjections(configuration))
                .AddEvents(o => o.Register(EventHelper.AllEvents))
                .AddCommands()
                .AddEventBus(configuration)
                .AddJsonSerializers();

            return services;
        }
    }
}

