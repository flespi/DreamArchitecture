using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using CleanArchitecture.Infrastructure.Cache.Serializers;
using Microsoft.Extensions.Caching.Hybrid;

namespace CleanArchitecture.Infrastructure.Cache;

internal static class HybridCacheSerializerFactory
{
    public static IHybridCacheSerializerFactory Json { get; }

    static HybridCacheSerializerFactory()
    {
        var cacheSerializerOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };

        Json = new HybridCacheJsonSerializerFactory(cacheSerializerOptions);
    }
}
