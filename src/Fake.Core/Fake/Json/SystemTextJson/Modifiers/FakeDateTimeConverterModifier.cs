using System.Diagnostics;
using System.Text.Json.Serialization.Metadata;
using Fake.Helpers;
using Fake.Json.SystemTextJson.Converters;
using Fake.Timing;

namespace Fake.Json.SystemTextJson.Modifiers;

public class FakeDateTimeConverterModifier
{
    private IServiceProvider _serviceProvider = null!;

    public Action<JsonTypeInfo> CreateModifyAction(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        return Modify;
    }

    private void Modify(JsonTypeInfo jsonTypeInfo)
    {
        Debug.Assert(_serviceProvider != null, nameof(_serviceProvider));

        if (_serviceProvider is null) throw new InvalidOperationException($"{nameof(_serviceProvider)} is null");

        if (ReflectionHelper.GetAttributeOrNull<DisableClockNormalizationAttribute>(jsonTypeInfo.Type) != null)
        {
            return;
        }

        foreach (var property in jsonTypeInfo.Properties
                     .Where(x => x.PropertyType == typeof(DateTime))
                )
        {
            // 如果属性上没有 DisableClockNormalizationAttribute 特性，则使用 DateTimeConverter
            if (property.AttributeProvider == null ||
                !property.AttributeProvider.GetCustomAttributes(typeof(DisableClockNormalizationAttribute), false)
                    .Any())
            {
                property.CustomConverter = _serviceProvider.GetRequiredService<DateTimeConverter>();
            }
        }
    }
}