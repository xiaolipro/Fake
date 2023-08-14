namespace Fake.Data;

public static class HasExtraPropertiesExtensions
{
    public static bool HasProperty(this IHasExtraProperties source, string name)
    {
        return source.ExtraProperties.ContainsKey(name);
    }
}