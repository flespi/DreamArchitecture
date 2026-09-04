using System.Security.Claims;

namespace CleanArchitecture.Application.Common.Identity;

public class IdentityAccessor(IIdentityResolver resolver) : IIdentityAccessor
{
    private IdentityContext Context
    {
        get
        {
            field ??= new(resolver.Principal);
            return field;
        }
    }

    public ClaimsPrincipal? Principal => Context.Principal;

    public IImpersonation Impersonate(ClaimsPrincipal? principal)
        => new Impersonation(Context, principal);
}
