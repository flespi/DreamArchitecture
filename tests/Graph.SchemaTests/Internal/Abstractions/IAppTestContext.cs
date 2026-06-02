using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Graph.SchemaTests.Internal.Abstractions;

public interface IAppTestContext : IAsyncEcoLifetime
{
    IServiceScopeFactory ServiceScopeFactory { get; }
}
