using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Graph.FunctionalTests.Internal.Abstractions;

public interface IAppTestContext : IAsyncEcoLifetime
{
    IServiceScopeFactory ServiceScopeFactory { get; }
}
