using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.Extensions.Caching.Hybrid;

namespace CleanArchitecture.Infrastructure.Cache.Serializers;

internal class HybridCacheJsonSerializerFactory : IHybridCacheSerializerFactory
{
    private readonly JsonSerializerOptions _options;

    public HybridCacheJsonSerializerFactory(JsonSerializerOptions options)
    {
        _options = options;
    }

    public bool TryCreateSerializer<T>([NotNullWhen(true)] out IHybridCacheSerializer<T>? serializer)
    {
        serializer = new HybridCacheJsonSerializer<T>(_options);
        return true;
    }
}
