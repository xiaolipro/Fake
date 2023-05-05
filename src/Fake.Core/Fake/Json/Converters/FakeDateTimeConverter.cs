using System.Text.Json;
using System.Text.Json.Serialization;
using Fake.Timing;
using Microsoft.Extensions.Options;

namespace Fake.Json.Converters;

public class FakeDateTimeConverter:JsonConverter<DateTime>
{
    private readonly IFakeClock _clock;
    private readonly FakeJsonOptions _options;

    public FakeDateTimeConverter(IFakeClock fakeClock, IOptions<FakeJsonOptions> abpJsonOptions)
    {
        _clock = fakeClock;
        _options = abpJsonOptions.Value;
    }

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}