using System.Text.Json.Serialization.Metadata;

namespace Fake.Json.SystemTextJson.Modifiers;

public class FakeSystemTextJsonModifiersOptions
{
    public List<Action<JsonTypeInfo>> Modifiers { get; }

    public FakeSystemTextJsonModifiersOptions()
    {
        Modifiers = new List<Action<JsonTypeInfo>>();
    }
}