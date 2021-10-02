using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIO.Domain;
using SIO.Domain.EventPublications.Projections;
using SIO.Infrastructure.Azure.ServiceBus.Extensions;
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
            services
                .AddSIOInfrastructure()
                    .AddEntityFrameworkCoreSqlServer(options => {
                        options.AddStore(configuration.GetConnectionString("Store"), o => o.MigrationsAssembly($"{nameof(SIO)}.{nameof(Migrations)}"));
                        options.AddProjections(configuration.GetConnectionString("Projection"), o => o.MigrationsAssembly($"{nameof(SIO)}.{nameof(Migrations)}"));
                    })
                    .AddEntityFrameworkCoreStoreProjector<EventPublicationQueue>(o => o.Interval = 2000)
                    .AddEntityFrameworkCoreStoreProjector<EventPublicationFailure>(o => o.Interval = 5000)
                    .AddEvents(o =>
                    {
                        o.Register(EventHelper.AllEvents);
                    })
                    .AddCommands()
                    .AddAzureServiceBus(o => {
                        o.UseConnection(configuration.GetValue<string>("AzureServiceBus"));
                        o.UseTopic(t =>
                        {
                            t.WithName("generic");
                        });
                    })
                    .AddJsonSerializers();

            return services;
        }
    }
}
