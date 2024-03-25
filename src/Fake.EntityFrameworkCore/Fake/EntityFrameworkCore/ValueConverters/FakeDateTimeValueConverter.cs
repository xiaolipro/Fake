namespace Fake.EntityFrameworkCore.ValueConverters;

public class FakeDateTimeValueConverter : ValueConverter<DateTime, DateTime>
{
    public FakeDateTimeValueConverter(IFakeClock fakeClock, ConverterMappingHints? mappingHints = null)
        : base(
            x => fakeClock.Normalize(x),
            x => fakeClock.Normalize(x), mappingHints)
    {
    }
}

public class FakeNullableDateTimeValueConverter : ValueConverter<DateTime?, DateTime?>
{
    public FakeNullableDateTimeValueConverter(IFakeClock fakeClock, ConverterMappingHints? mappingHints = null)
        : base(
            x => x.HasValue ? fakeClock.Normalize(x.Value) : x,
            x => x.HasValue ? fakeClock.Normalize(x.Value) : x, mappingHints)
    {
    }
}