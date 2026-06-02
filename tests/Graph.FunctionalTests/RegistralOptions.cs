using DotNet.Testcontainers.Containers;

namespace CleanArchitecture.Graph.FunctionalTests;

public class RegistralOptions
{
    public required IDatabaseContainer Container { get; init; }
}
