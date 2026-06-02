using CleanArchitecture.Application.FunctionalTests.Internal.Services;
using DotNet.Testcontainers.Containers;

namespace CleanArchitecture.Application.FunctionalTests;

public class RegistralOptions
{
    public required IdentityResolver IdentityResolver { get; init; }

    public required IDatabaseContainer Container { get; init; }
}
