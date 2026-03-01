using System.Security.Claims;
using Orca;

namespace CleanArchitecture.Application.FunctionalTests;

public class AuthManager
{
    private readonly IAuthorizationContextProvider _authContextProvider;

    public AuthManager(IAuthorizationContextProvider authContextProvider)
    {
        _authContextProvider = authContextProvider;
    }

    public async Task<ClaimsPrincipal> CreatePrincipalAsync(string subject, string userName, string[] roles)
    {
        var principal = new ClaimsPrincipal();

        var defaultIdentity = IdentityHelper.CreateIdentity(subject, userName, roles);
        principal.AddIdentity(defaultIdentity);

        var authContext = await _authContextProvider.CreateAsync(principal);

        var additionalIdentity = new ClaimsIdentityFactory(new ClaimTypeMap
        {
            NameClaimType = DefaultClaimTypes.Name,
            RoleClaimType = DefaultClaimTypes.Role,
        })
        .Create(authContext);

        principal.AddIdentity(additionalIdentity);

        return principal;
    }
}
