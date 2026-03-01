using System.Security.Claims;

namespace CleanArchitecture.Application.Common.Identity;

public class IdentityContext(ClaimsPrincipal? principal)
{
    public ClaimsPrincipal? Principal { get; private set; } = principal;

    public void Swap(ClaimsPrincipal? current, out ClaimsPrincipal? previous)
    {
        previous = Principal;
        Principal = current;
    }
}
