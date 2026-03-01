using System.Security.Claims;

namespace CleanArchitecture.Application.Common.Identity;

/// <summary>
/// Represents a scope of impersonation for a <see cref="ClaimsPrincipal"/>.
/// </summary>
public interface IImpersonation : IDisposable
{
}
