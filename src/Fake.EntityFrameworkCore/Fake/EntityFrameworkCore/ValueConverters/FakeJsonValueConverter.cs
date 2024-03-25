using System.Text.Json;
using Fake.Json.SystemTextJson.Converters;

namespace Fake.EntityFrameworkCore.ValueConverters;

public class FakeJsonValueConverter<TPropertyType> : ValueConverter<TPropertyType, string>
{
    public FakeJsonValueConverter()
        : base(
            d => SerializeObject(d),
            s => DeserializeObject(s))
    {
    }

    private static string SerializeObject(TPropertyType property)
    {
        return JsonSerializer.Serialize(property);
    }

    private static TPropertyType DeserializeObject(string propertyJson)
    {
        return JsonSerializer.Deserialize<TPropertyType>(propertyJson, new JsonSerializerOptions()
        {
            Converters =
            {
                new ObjectToInferredTypesConverter()
            }
        })!;
    }
}