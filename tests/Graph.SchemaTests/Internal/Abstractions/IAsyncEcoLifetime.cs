namespace CleanArchitecture.Graph.SchemaTests.Internal.Abstractions;

public interface IAsyncEcoLifetime : IAsyncLifetime
{
    ValueTask ResetAsync();
}
