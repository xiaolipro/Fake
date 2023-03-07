using System;
using System.Linq.Expressions;
using Fake.Timing;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fake.EntityFrameworkCore.ValueConverters;

public class FakeDateTimeValueConverter:ValueConverter<DateTime,DateTime>
{
    public FakeDateTimeValueConverter(IClock clock, [CanBeNull] ConverterMappingHints mappingHints = null)
        : base(
            x => clock.Normalize(x),
            x => clock.Normalize(x), mappingHints)
    {
    }
}

public class FakeNullableDateTimeValueConverter : ValueConverter<DateTime?, DateTime?>
{
    public FakeNullableDateTimeValueConverter(IClock clock, [CanBeNull] ConverterMappingHints mappingHints = null)
        : base(
            x => x.HasValue ? clock.Normalize(x.Value) : x,
            x => x.HasValue ? clock.Normalize(x.Value) : x, mappingHints)
    {
    }
}
