using System.Text.Json.Serialization.Metadata;
using Fake.Json.SystemTextJson.Modifiers;
using Microsoft.Extensions.Options;

namespace Fake.Json.SystemTextJson;

public class FakeDefaultJsonTypeInfoResolver : DefaultJsonTypeInfoResolver
{
    public FakeDefaultJsonTypeInfoResolver(IOptions<FakeSystemTextJsonModifiersOptions> options)
    {
        foreach (var modifier in options.Value.Modifiers)
        {
            Modifiers.Add(modifier);
        }
    }
}