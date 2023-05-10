using System.Text.Json.Serialization.Metadata;

namespace Fake.Json.SystemTextJson.Modifiers;

public class FakeSystemTextJsonModifiersOption
{
    public List<Action<JsonTypeInfo>> Modifiers { get; }

    public FakeSystemTextJsonModifiersOption()
    {
        Modifiers = new List<Action<JsonTypeInfo>>();
    }
}