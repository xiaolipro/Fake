using Fake.Domain;

namespace Fake.EntityFrameworkCore.ValueConverters;

public class FakeEnumerationValueConverter<TEnumeration> : ValueConverter<TEnumeration, int>
    where TEnumeration : Enumeration
{
    public FakeEnumerationValueConverter(ConverterMappingHints? mappingHints = null)
        : base(
            x => x.Value,
            x => Enumeration.FromValue<TEnumeration>(x),
            mappingHints)
    {
    }
}