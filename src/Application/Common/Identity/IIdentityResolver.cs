using System.Security.Claims;

namespace CleanArchitecture.Application.Common.Identity;

/// <summary>
/// Resolves the current security principal from the execution context.
/// </summary>
public interface IIdentityResolver
{
    /// <summary>
    /// Gets the current <see cref="ClaimsPrincipal"/>, if available.
    /// </summary>
    ClaimsPrincipal? Principal { get; }
}
