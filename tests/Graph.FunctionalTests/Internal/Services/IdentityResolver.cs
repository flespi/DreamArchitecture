using System.Security.Claims;
using CleanArchitecture.Application.Common.Identity;

namespace CleanArchitecture.Graph.FunctionalTests.Internal.Services;

public class IdentityResolver : IIdentityResolver
{
    public ClaimsPrincipal? Principal { get; set; }
}
