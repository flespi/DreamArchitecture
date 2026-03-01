using System.Security.Claims;
using CleanArchitecture.Shared.FunctionalTests;
using Microsoft.Extensions.DependencyInjection;
using Orca;

namespace CleanArchitecture.Graph.FunctionalTests;

// [Collection(WebApplicationTestCollection.Name)]
public abstract class WebApplicationTest : WebApplicationTest<Program>, IClassFixture<WebApplicationContext>
{
    private readonly IdentityResolver _identityResolver = new();
    private IGraphClient _client = null!;

    public ClaimsPrincipal? Principal => _identityResolver.Principal;
    public IGraphClient Client => _client;

    public WebApplicationTest(WebApplicationContext context) : base(context)
    {
    }

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();

        _client = CreateGraphQLCliente();
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
        using var scope = Context.CreateServiceScope();

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
