using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.Json;
using Fake.Json.SystemTextJson.JsonConverters;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fake.EntityFrameworkCore.ValueConverters;

public class FakeJsonValueConverter<TPropertyType>:ValueConverter<TPropertyType, string>
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
        return JsonSerializer.Deserialize<TPropertyType>(propertyJson, DeserializeOptions);
    }
    
    private static readonly JsonSerializerOptions DeserializeOptions = new JsonSerializerOptions()
    {
        Converters =
        {
            new ObjectToInferredTypesConverter()
        }
    };
}