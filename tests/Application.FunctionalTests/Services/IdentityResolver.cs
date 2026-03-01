using System.Security.Claims;
using CleanArchitecture.Application.Common.Identity;

namespace CleanArchitecture.Application.FunctionalTests;

public class IdentityResolver : IIdentityResolver
{
    public ClaimsPrincipal? Principal { get; set; }
}
