using System.Security.Claims;
using CleanArchitecture.Application;

namespace CleanArchitecture.Graph.SchemaTests.Internal.Helpers;

public static class OperationRequestHelper
{
    public static ClaimsPrincipal CreateUser(ClaimsIdentity? identity)
    {
        var principal = new ClaimsPrincipal();
        if (identity is not null)
        {
            var claims = identity.Claims;
            identity = new ClaimsIdentity(claims, "Test", DefaultClaimTypes.Name, DefaultClaimTypes.Role);
            principal.AddIdentity(identity);
        }
        return principal;
    }
}
