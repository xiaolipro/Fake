using System.Text.Json.Serialization.Metadata;
using Fake.Json.SystemTextJson.Converters;
using Fake.Reflection;
using Fake.Timing;

namespace Fake.Json.SystemTextJson.Modifiers;

public class FakeDateTimeConverterModifier
{
    private IServiceProvider _serviceProvider;

    public Action<JsonTypeInfo> CreateModifyAction(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        return Modify;
    }
    
    private void Modify(JsonTypeInfo jsonTypeInfo)
    {
        if (ReflectionHelper.GetAttributeOrDefault<DisableClockNormalizationAttribute>(jsonTypeInfo.Type) != null)
        {
            return;
        }

        foreach (var property in jsonTypeInfo.Properties
                     .Where(x => x.PropertyType == typeof(DateTime))
                 )
        {
            // 如果属性上没有 DisableClockNormalizationAttribute 特性，则使用 FakeDateTimeConverter
            if (property.AttributeProvider == null ||
                !property.AttributeProvider.GetCustomAttributes(typeof(DisableClockNormalizationAttribute), false).Any())
            {
                property.CustomConverter = _serviceProvider.GetRequiredService<FakeDateTimeConverter>();
            }
        }
    }
}