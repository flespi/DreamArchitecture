using System.Security.Claims;

namespace CleanArchitecture.Application.Common.Identity;

/// <summary>
/// Provides access to the currently impersonated security principal.
/// </summary>
public interface IIdentityAccessor
{
    /// <summary>
    /// Gets the current <see cref="ClaimsPrincipal"/>, if available.
    /// </summary>
    ClaimsPrincipal? Principal { get; }

    /// <summary>
    /// Begins an impersonation scope using the specified principal.
    /// </summary>
    /// <param name="principal">The principal to impersonate.</param>
    /// <returns>
    /// An <see cref="IImpersonation"/> scope.
    /// </returns>
    IImpersonation Impersonate(ClaimsPrincipal principal);
}
