using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Graph.FunctionalTests.Internal.Transport;

public static class GraphClientFactory
{
    public static IGraphClient Create(HttpClientResolver httpClientResolver)
    {
        var services = new ServiceCollection();

        services.AddGraphClient();
        services.AddSingleton<IHttpClientFactory>(new HttpClientFactory(httpClientResolver));

        var serviceProvider = services.BuildServiceProvider();

        return serviceProvider.GetRequiredService<IGraphClient>();
    }
}
