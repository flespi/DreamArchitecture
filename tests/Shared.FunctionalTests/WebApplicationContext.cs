using System.Data.Common;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Respawn;

namespace CleanArchitecture.Shared.FunctionalTests;

public abstract class WebApplicationContext<TEntryPoint> : IAsyncLifetime
    where TEntryPoint : class
{
    protected IDatabaseContainer Container { get; }
    protected WebApplicationFactory<TEntryPoint> Factory { get; }

    private readonly Lazy<IServiceScopeFactory> _scopeFactory;

    private Respawner _snapshot = null!;

    public WebApplicationContext()
    {
        Container = CreateDatabaseContainer();
        Factory = CreateWebApplicationFactory(Container);

        // This needs to be evaluated after InitializeAsync
        _scopeFactory = new(CreateScopeFactory);
    }

    public virtual async ValueTask InitializeAsync()
    {
        await Container.StartAsync();

        _snapshot = await CreateSnapshot();
    }

    public virtual async ValueTask DisposeAsync()
    {
        await Container.StopAsync();
    }

    public virtual async Task ResetState()
    {
        using var connection = CreateDatabaseConnection();
        await connection.OpenAsync();

        await _snapshot.ResetAsync(connection);

        await InitializeData();
    }

    public IServiceScope CreateServiceScope()
    {
        return _scopeFactory.Value.CreateScope();
    }

    protected abstract IDatabaseContainer CreateDatabaseContainer();

    protected abstract DbConnection CreateDatabaseConnection();

    protected abstract WebApplicationFactory<TEntryPoint> CreateWebApplicationFactory(IDatabaseContainer container);

    protected virtual Task InitializeData() => Task.CompletedTask;

    public HttpClient CreateHttpClient(params DelegatingHandler[] handlers)
        => Factory.CreateDefaultClient(handlers);

    private IServiceScopeFactory CreateScopeFactory()
        => Factory.Services.GetRequiredService<IServiceScopeFactory>();

    private async Task<Respawner> CreateSnapshot()
    {
        await InitializeData();

        using var connection = CreateDatabaseConnection();
        await connection.OpenAsync();

        return await Respawner.CreateAsync(connection, new()
        {
            TablesToIgnore = ["__EFMigrationsHistory"]
        });
    }
}
