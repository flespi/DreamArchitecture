using System.Security.Claims;
using CleanArchitecture.Application.Common.Identity;
using CleanArchitecture.Graph.SchemaTests.Internal.Extensions;

namespace CleanArchitecture.Graph.SchemaTests.Internal.Services;

public class IdentityResolver : IIdentityResolver
{
    private readonly IRequestContextAccessor _requestContextAccessor;

    public IdentityResolver(IRequestContextAccessor requestContextAccessor)
    {
        _requestContextAccessor = requestContextAccessor;
    }

    public ClaimsPrincipal? Principal => _requestContextAccessor.TryGetRequestContext(out var requestContext) ? GetUser(requestContext) : null;

    private ClaimsPrincipal? GetUser(RequestContext? requestContext)
    {
        if (requestContext?.ContextData is not null)
        {
            if (requestContext.ContextData.TryGetValue(nameof(ClaimsPrincipal), out var value))
            {
                if (value is ClaimsPrincipal principal)
                {
                    return principal;
                }
            }
        }

        return null;
    }
}
