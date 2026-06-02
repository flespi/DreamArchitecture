using CleanArchitecture.Application.Common.Identity;
using CleanArchitecture.Application.Common.Processing;
using CleanArchitecture.Application.FunctionalTests.Internal.Services;
using CleanArchitecture.Infrastructure.Data;
using EFSeeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CleanArchitecture.Application.FunctionalTests;

public static class DependencyInjection
{
    public static void AddTestServices(this IServiceCollection services, RegistralOptions registralOptions)
    {
        var configuration = new ConfigurationBuilder().Build();
        services.AddSingleton<IConfiguration>(configuration);

        services.AddLogging();

        services
            .RemoveDbContext<ApplicationDbContext>()
            .AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                var connectionString = registralOptions.Container.GetConnectionString();
                var interceptors = sp.GetServices<ISaveChangesInterceptor>();

                var seeder = sp.GetRequiredService<DbContextSeeder<ApplicationDbContext>>();

                options
                    .UseSqlServer(connectionString)
                    .AddInterceptors(interceptors)
                    .UseAsyncSeeding(seeder);
            });

        services
            .RemoveAll<IIdentityResolver>()
            .AddSingleton<IIdentityResolver>(registralOptions.IdentityResolver);

        services
            .RemoveAll<IIdempotentRequest>()
            .AddScoped<IIdempotentRequest, IdempotentRequest>();
    }
}
