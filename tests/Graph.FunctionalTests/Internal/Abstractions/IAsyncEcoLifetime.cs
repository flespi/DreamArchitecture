namespace CleanArchitecture.Graph.FunctionalTests.Internal.Abstractions;

public interface IAsyncEcoLifetime : IAsyncLifetime
{
    ValueTask ResetAsync();
}
