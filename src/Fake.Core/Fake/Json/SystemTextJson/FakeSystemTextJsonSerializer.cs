using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Fake.Json.SystemTextJson;

public class FakeSystemTextJsonSerializer : IFakeJsonSerializer
{
    private static readonly ConcurrentDictionary<object, JsonSerializerOptions> OptionsCache = new();
    private readonly JsonSerializerOptions _options;

    public FakeSystemTextJsonSerializer(IOptions<JsonSerializerOptions> options)
    {
        _options = options.Value;
    }

    public string Serialize(object obj, bool camelCase = true, bool indented = false)
    {
        return JsonSerializer.Serialize(obj, GetJsonSerializerOptions(camelCase, indented));
    }

    public T? Deserialize<T>(string jsonString, bool camelCase = true)
    {
        return JsonSerializer.Deserialize<T>(jsonString, GetJsonSerializerOptions(camelCase));
    }

    public object? Deserialize(Type type, string jsonString, bool camelCase = true)
    {
        return JsonSerializer.Deserialize(jsonString, type, GetJsonSerializerOptions(camelCase));
    }

    protected virtual JsonSerializerOptions GetJsonSerializerOptions(bool camelCase = true, bool indented = false)
    {
        return OptionsCache.GetOrAdd(new
        {
            camelCase,
            indented,
            _options
        }, _ =>
        {
            var settings = new JsonSerializerOptions(_options);

            if (camelCase)
            {
                settings.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            }

            if (indented)
            {
                settings.WriteIndented = true;
            }

            return settings;
        });
    }
}