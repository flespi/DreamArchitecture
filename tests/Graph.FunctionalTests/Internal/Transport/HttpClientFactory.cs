namespace CleanArchitecture.Graph.FunctionalTests.Internal.Transport;

public delegate HttpClient HttpClientResolver(string name);

public class HttpClientFactory(HttpClientResolver httpClientResolver) : IHttpClientFactory
{
    public HttpClient CreateClient(string name) => httpClientResolver(name);
}
