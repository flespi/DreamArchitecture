using CleanArchitecture.Graph.SchemaTests.Internal.Abstractions;
using CleanArchitecture.Infrastructure.Data;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Graph.SchemaTests;

public class AppTestContext : IAppTestContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IRequestExecutorBuilder _executorBuilder;

    public IServiceScopeFactory ServiceScopeFactory => _serviceProvider.GetRequiredService<IServiceScopeFactory>();

    public AppTestContext()
    {
        _serviceProvider = RegisterServices(out _executorBuilder).BuildServiceProvider();
    }

    private IServiceCollection RegisterServices(out IRequestExecutorBuilder executorBuilder)
    {
        var services = new ServiceCollection();

        services.AddApplicationServices();
        services.AddInfrastructureServices();
        services.AddGraphServices(out executorBuilder);
        services.AddTestServices();

        executorBuilder.AddDataLoaders();

        executorBuilder.ModifyRequestOptions(options =>
        {
            options.IncludeExceptionDetails = true;
        });

        return services;
    }

    public async ValueTask InitializeAsync()
    {
        await SeedDataAsync();
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    public async ValueTask ResetAsync()
    {
        await SeedDataAsync();
    }

    private async Task SeedDataAsync()
    {
        using var scope = ServiceScopeFactory.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }

    public async Task<IRequestExecutor> CreateRequestExecutorAsync()
    {
        return await _executorBuilder.BuildRequestExecutorAsync();
    }
}
