namespace CleanArchitecture.Graph.FunctionalTests;

public delegate HttpClient HttpClientResolver(string name);

public class HttpClientFactory(HttpClientResolver httpClientResolver) : IHttpClientFactory
{
    public HttpClient CreateClient(string name) => httpClientResolver(name);
}
