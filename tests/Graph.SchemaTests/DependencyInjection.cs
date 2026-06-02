using CleanArchitecture.Application.Common.Identity;
using CleanArchitecture.Application.Common.Processing;
using CleanArchitecture.Graph.SchemaTests.Internal.Services;
using CleanArchitecture.Infrastructure.Data;
using EFSeeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Graph.SchemaTests;

public static class DependencyInjection
{
    public static void AddTestServices(this IServiceCollection services)
    {
        var databaseName = Guid.NewGuid().ToString();

        var configuration = new ConfigurationBuilder().Build();
        services.AddSingleton<IConfiguration>(configuration);

        services.AddLogging();

        services
            .RemoveDbContext<ApplicationDbContext>()
            .AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                var interceptors = sp.GetServices<ISaveChangesInterceptor>();

                var seeder = sp.GetRequiredService<DbContextSeeder<ApplicationDbContext>>();

                options
                    .UseInMemoryDatabase(databaseName)
                    .AddInterceptors(interceptors)
                    .UseAsyncSeeding(seeder);
            });

        services.AddScoped<IIdentityResolver, IdentityResolver>();
        services.AddScoped<IIdempotentRequest, IdempotentRequest>();
    }
}
