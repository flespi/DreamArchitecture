using System.Security.Claims;

namespace CleanArchitecture.Application.Common.Identity;

public class Impersonation : IImpersonation
{
    private bool _disposed = false;

    private readonly IdentityContext _context;

    private readonly ClaimsPrincipal? _previousPrincipal;
    private readonly ClaimsPrincipal? _newPrincipal;

    public ClaimsPrincipal? Principal => _newPrincipal;

    public Impersonation(IdentityContext context, ClaimsPrincipal? principal)
    {
        _context = context;
        _newPrincipal = principal;

        _context.Swap(_newPrincipal, out _previousPrincipal);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _context.Swap(_previousPrincipal, out ClaimsPrincipal? _);
            _disposed = true;
        }
    }
}
