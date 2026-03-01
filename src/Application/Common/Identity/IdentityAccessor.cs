using System.Security.Claims;

namespace CleanArchitecture.Application.Common.Identity;

public class IdentityAccessor(IIdentityResolver resolver) : IIdentityAccessor
{
    private readonly IdentityContext _context = new(resolver.Principal);

    public ClaimsPrincipal? Principal => _context.Principal;

    public IImpersonation Impersonate(ClaimsPrincipal? principal)
        => new Impersonation(_context, principal);
}
