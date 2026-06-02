using CleanArchitecture.Application.FunctionalTests.Internal.Utils;
using CleanArchitecture.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orca;

namespace CleanArchitecture.Application.FunctionalTests;

// [Collection(AppTestCollection.Name)]
public abstract class BaseTest : IAsyncLifetime, IClassFixture<AppTestContext>
{
    protected AppTestContext Context { get; }

    public BaseTest(AppTestContext context)
    {
        Context = context;
    }

    public async ValueTask InitializeAsync()
    {
        await Context.ResetAsync();
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = Context.ServiceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public async Task SendAsync(IBaseRequest request)
    {
        using var scope = Context.ServiceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        await mediator.Send(request);
    }

    public async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = Context.ServiceScopeFactory.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = Context.ServiceScopeFactory.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using var scope = Context.ServiceScopeFactory.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().CountAsync();
    }

    public async Task<string> RunAsDefaultUserAsync()
    {
        return await RunAsUserAsync("test@local", []);
    }

    public async Task<string> RunAsAdministratorAsync()
    {
        return await RunAsUserAsync("administrator@local", ["Administrator"]);
    }

    private async Task<string> RunAsUserAsync(string userName, string[] roles)
    {
        using var scope = Context.ServiceScopeFactory.CreateScope();

        var authStores = scope.ServiceProvider.GetRequiredService<IOrcaStoreAccessor>();
        var authContextProvider = scope.ServiceProvider.GetRequiredService<IAuthorizationContextProvider>();

        var identityManager = new IdentityManager(authStores);
        var authManager = new AuthManager(authContextProvider);

        var subject = await identityManager.CreateSubjectAsync(userName, roles);
        var principal = await authManager.CreatePrincipalAsync(subject, userName, roles);

        Context.IdentityResolver.Principal = principal;

        return subject;
    }
}
