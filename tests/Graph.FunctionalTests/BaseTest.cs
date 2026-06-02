using System.Security.Claims;
using CleanArchitecture.Graph.FunctionalTests.Internal.Services;
using CleanArchitecture.Graph.FunctionalTests.Internal.Transport;
using CleanArchitecture.Graph.FunctionalTests.Internal.Utils;
using Microsoft.Extensions.DependencyInjection;
using Orca;

namespace CleanArchitecture.Graph.FunctionalTests;

// [Collection(AppTestCollection.Name)]
public abstract class BaseTest : IAsyncLifetime, IClassFixture<AppTestContext>
{
    private readonly IdentityResolver _identityResolver = new();
    private IGraphClient _client = null!;

    public ClaimsPrincipal? Principal => _identityResolver.Principal;
    public IGraphClient Client => _client;

    public AppTestContext Context { get; set; }

    public BaseTest(AppTestContext context)
    {
        Context = context;
    }

    public async ValueTask InitializeAsync()
    {
        await Context.ResetAsync();

        _client = CreateGraphQLCliente();
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    private IGraphClient CreateGraphQLCliente()
    {
        return GraphClientFactory.Create(_ => CreateClient());

        HttpClient CreateClient()
        {
            var authHandler = new AuthorizationMessageHandler(_identityResolver);

            var httpClient = Context.CreateHttpClient(authHandler);
            httpClient.BaseAddress = new Uri(httpClient.BaseAddress!, "graphql");
            return httpClient;
        }
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

        var subjectStore = scope.ServiceProvider.GetRequiredService<ISubjectStore>();
        var authContextProvider = scope.ServiceProvider.GetRequiredService<IAuthorizationContextProvider>();

        var subjectManager = new SubjectManager(subjectStore);
        var authManager = new AuthManager(authContextProvider);

        var subject = await subjectManager.CreateSubjectAsync(userName, roles);
        var principal = await authManager.CreatePrincipalAsync(subject, userName, roles);

        _identityResolver.Principal = principal;

        return subject;
    }
}
