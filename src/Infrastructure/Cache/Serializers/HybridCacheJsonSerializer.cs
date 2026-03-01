using System.Buffers;
using System.Text.Json;
using Microsoft.Extensions.Caching.Hybrid;

namespace CleanArchitecture.Infrastructure.Cache.Serializers;

internal class HybridCacheJsonSerializer<T> : IHybridCacheSerializer<T>
{
    private readonly JsonSerializerOptions _options;

    public HybridCacheJsonSerializer(JsonSerializerOptions options)
    {
        _options = options;
    }

    public T Deserialize(ReadOnlySequence<byte> source)
    {
        var reader = new Utf8JsonReader(source);
        return JsonSerializer.Deserialize<T>(ref reader, _options)!;
    }

    public void Serialize(T value, IBufferWriter<byte> target)
    {
        using var writer = new Utf8JsonWriter(target);
        JsonSerializer.Serialize(writer, value, _options);
    }
}
