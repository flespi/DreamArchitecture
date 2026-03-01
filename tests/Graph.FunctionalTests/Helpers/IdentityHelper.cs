using System.Security.Claims;
using CleanArchitecture.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Orca;

namespace CleanArchitecture.Graph.FunctionalTests;

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

        return new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme, DefaultClaimTypes.Name, DefaultClaimTypes.Role);
    }
}
