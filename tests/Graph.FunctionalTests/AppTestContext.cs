using CleanArchitecture.Graph.FunctionalTests.Internal.Abstractions;
using CleanArchitecture.Graph.FunctionalTests.Internal.Snapshot;
using CleanArchitecture.Infrastructure.Data;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace CleanArchitecture.Graph.FunctionalTests;

public class AppTestContext : IAppTestContext
{
    private readonly IDatabaseContainer _container;
    private readonly IAsyncEcoLifetime _snapshot;

    private readonly WebApplicationFactory _factory;

    public IServiceScopeFactory ServiceScopeFactory => _factory.Services.GetRequiredService<IServiceScopeFactory>();

    public AppTestContext()
    {
        _snapshot = CreateTestDatabase(out _container);

        _factory = WebApplicationFactory.Create(options =>
        {
            options.ConfigureTestServices(services =>
            {
                services.AddTestServices(new()
                {
                    Container = _container,
                });
            });
        });
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
        await _snapshot.ResetAsync();
        await SeedDataAsync();
    }

    private async Task SeedDataAsync()
    {
        using var scope = ServiceScopeFactory.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();
    }

    public HttpClient CreateHttpClient(params DelegatingHandler[] handlers)
    {
        return _factory.CreateDefaultClient(handlers);
    }
}
