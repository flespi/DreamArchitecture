using CleanArchitecture.Application;
using CleanArchitecture.Application.Common.Data;
using CleanArchitecture.Infrastructure.Cache;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Data.Abstractions;
using CleanArchitecture.Infrastructure.Data.Aggregates;
using CleanArchitecture.Infrastructure.Data.Interceptors;
using EFSeeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Orca;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, ConcurrentEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContextSeeder<ApplicationDbContext>(options =>
        {
            options.DataSeedersAssembly(typeof(ApplicationDbContext).Assembly);
        });

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("CleanArchitectureDb");

            ArgumentNullException.ThrowIfNull(connectionString);

            var seeder = sp.GetRequiredService<DbContextSeeder<ApplicationDbContext>>();
            var interceptors = sp.GetServices<ISaveChangesInterceptor>();

            options.UseSqlServer(connectionString)
                .UseAsyncSeeding(seeder)
                .AddInterceptors(interceptors);

            options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        services.AddHybridCache()
            .AddSerializerFactory(HybridCacheSerializerFactory.Json);

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAggregateBoundaryProvider, AggregateBoundaryProvider>();

        services.AddSingleton(TodoListAggregate.Boundary);

        services
            .AddOrca(options =>
            {
                options.ClaimTypeMap = new ClaimTypeMap
                {
                    NameClaimType = DefaultClaimTypes.Name,
                    RoleClaimType = DefaultClaimTypes.Role,
                };

                options.ClaimTypeMap.AllowedSubjectClaimTypes.Clear();
                options.ClaimTypeMap.AllowedSubjectClaimTypes.Add(DefaultClaimTypes.Subject);
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddAuthorization()
            .AddAuthorizationCache();

        services.AddSingleton(TimeProvider.System);
    }
}
