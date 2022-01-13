using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SIO.Domain.Extensions;
using SIO.EntityFrameworkCore.DbContexts;
using SIO.EventPublisher.Extensions;
using SIO.Infrastructure.EntityFrameworkCore.DbContexts;
using SIO.Infrastructure.EntityFrameworkCore.Extensions;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddInfrastructure(hostContext.Configuration)
            .AddDomain();
    })
    .Build();

var env = host.Services.GetRequiredService<IHostEnvironment>();

if (env.IsDevelopment())
{
    await host.RunProjectionMigrationsAsync();
    await host.RunStoreMigrationsAsync<SIOStoreDbContext>();
    await host.RunStoreMigrationsAsync<SIOEventPublisherStoreDbContext>();
}

await host.RunAsync();
