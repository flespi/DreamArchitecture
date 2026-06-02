using CleanArchitecture.Application.Common.Identity;
using CleanArchitecture.Graph.FunctionalTests.Internal.Helpers;

namespace CleanArchitecture.Graph.FunctionalTests.Internal.Transport;

public class AuthorizationMessageHandler : DelegatingHandler
{
    private readonly IIdentityResolver _identityResolver;

    public AuthorizationMessageHandler(IIdentityResolver identityResolver)
    {
        _identityResolver = identityResolver;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_identityResolver.Principal is not null)
        {
            var token = JwtHelper.GenerateJwtToken(_identityResolver.Principal.Claims);
            request.Headers.Add("Authorization", $"Bearer {token}");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
