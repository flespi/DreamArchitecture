using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Shared.FunctionalTests;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orca;

namespace CleanArchitecture.Application.FunctionalTests;

// [Collection(WebApplicationTestCollection.Name)]
public abstract class WebApplicationTest : WebApplicationTest<Program>, IClassFixture<WebApplicationContext>
{
    private IdentityResolver IdentityResolver { get; }

    public WebApplicationTest(WebApplicationContext context) : base(context)
    {
        IdentityResolver = context.IdentityResolver;
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = Context.CreateServiceScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public async Task SendAsync(IBaseRequest request)
    {
        using var scope = Context.CreateServiceScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        await mediator.Send(request);
    }

    public async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
    where TEntity : class
    {
        using var scope = Context.CreateServiceScope();

        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = Context.CreateServiceScope();

        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using var scope = Context.CreateServiceScope();

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
        using var scope = Context.CreateServiceScope();

        var authStores = scope.ServiceProvider.GetRequiredService<IOrcaStoreAccessor>();
        var authContextProvider = scope.ServiceProvider.GetRequiredService<IAuthorizationContextProvider>();

        var identityManager = new IdentityManager(authStores);
        var authManager = new AuthManager(authContextProvider);

        var subject = await identityManager.CreateSubjectAsync(userName, roles);
        var principal = await authManager.CreatePrincipalAsync(subject, userName, roles);

        IdentityResolver.Principal = principal;

        return subject;
    }
}
