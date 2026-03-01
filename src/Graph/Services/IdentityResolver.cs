using System.Security.Claims;
using CleanArchitecture.Application.Common.Identity;

namespace CleanArchitecture.Graph.Services;

public class IdentityResolver : IIdentityResolver
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityResolver(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal? Principal => _httpContextAccessor.HttpContext?.User;
}
