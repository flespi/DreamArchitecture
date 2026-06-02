using CleanArchitecture.Application.FunctionalTests.Internal.Abstractions;
using CleanArchitecture.Application.FunctionalTests.Internal.Services;
using CleanArchitecture.Application.FunctionalTests.Internal.Snapshot;
using CleanArchitecture.Infrastructure.Data;
using DotNet.Testcontainers.Containers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace CleanArchitecture.Application.FunctionalTests;

public class AppTestContext : IAppTestContext
{
    private readonly IDatabaseContainer _container;
    private readonly IAsyncEcoLifetime _snapshot;

    private readonly IServiceProvider _serviceProvider;
    private readonly IdentityResolver _identityResolver = new();

    public IServiceScopeFactory ServiceScopeFactory => _serviceProvider.GetRequiredService<IServiceScopeFactory>();

    public IdentityResolver IdentityResolver => _identityResolver;

    public AppTestContext()
    {
        _snapshot = CreateTestDatabase(out _container);

        _serviceProvider = RegisterServices().BuildServiceProvider();

    }

    private IAsyncEcoLifetime CreateTestDatabase(out IDatabaseContainer container)
    {
        container = new MsSqlBuilder().Build();

        return new DatabaseSnapshotBuilder()
            .WithOptions(new()
            {
                TablesToIgnore = ["__EFMigrationsHistory"]
            })
            .Build(container, SqlClientFactory.Instance);
    }

    private IServiceCollection RegisterServices()
    {
        var services = new ServiceCollection();

        services.AddApplicationServices();
        services.AddInfrastructureServices();
        services.AddTestServices(new()
        {
            Container = _container,
            IdentityResolver = _identityResolver,
        });

        return services;
    }

    public async ValueTask InitializeAsync()
    {
        await _container.StartAsync();

        await SeedDataAsync();
        await _snapshot.InitializeAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _snapshot.DisposeAsync();
        await _container.StopAsync();
    }

    public async ValueTask ResetAsync()
    {
        _identityResolver.Principal = null;

        await _snapshot.ResetAsync();
        await SeedDataAsync();
    }

    private async Task SeedDataAsync()
    {
        using var scope = ServiceScopeFactory.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();
    }
}
