using System.Security.Claims;

namespace CleanArchitecture.Application;

public static class ClaimsPrincipalExtensions
{
    public static string? GetSubject(this ClaimsPrincipal principal)
        => principal?.FindFirst(DefaultClaimTypes.Subject)?.Value;

    public static string? GetName(this ClaimsPrincipal principal)
        => principal?.FindFirst(DefaultClaimTypes.Name)?.Value;
}
