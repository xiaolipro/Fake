using Fake.DomainDrivenDesign;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fake.EntityFrameworkCore.ValueConverters;

public class FakeEnumerationValueConverter : ValueConverter<Enumeration, int>
{
    public FakeEnumerationValueConverter(ConverterMappingHints? mappingHints = null)
        : base(
            x => x.Id,
            x => Enumeration.FromValue<Enumeration>(x),
            mappingHints)
    {
    }
}