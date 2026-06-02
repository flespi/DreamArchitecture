using System.Security.Claims;

namespace CleanArchitecture.Application.FunctionalTests.Internal.Helpers;

public static class IdentityHelper
{
    public static ClaimsIdentity CreateIdentity(string subject, string userName, string[] roles)
    {
        var claims = new List<Claim>
        {
            new(DefaultClaimTypes.Subject, subject),
            new(DefaultClaimTypes.Name, userName),
        };

        foreach (var role in roles)
        {
            claims.Add(new(DefaultClaimTypes.Role, role));
        }

        return new ClaimsIdentity(claims, "Test", DefaultClaimTypes.Name, DefaultClaimTypes.Role);
    }
}
