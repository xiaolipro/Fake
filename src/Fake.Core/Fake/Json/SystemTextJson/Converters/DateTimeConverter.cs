using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Fake.Timing;
using Microsoft.Extensions.Options;

namespace Fake.Json.SystemTextJson.Converters;

public class DateTimeConverter : JsonConverter<DateTime>
{
    private readonly IFakeClock _clock;
    private readonly FakeJsonSerializerOptions _serializerOptions;

    public DateTimeConverter(IFakeClock fakeClock, IOptions<FakeJsonSerializerOptions> options)
    {
        _clock = fakeClock;
        _serializerOptions = options.Value;
    }

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (_serializerOptions.InputDateTimeFormats.Any())
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                foreach (var format in _serializerOptions.InputDateTimeFormats)
                {
                    var s = reader.GetString();
                    if (DateTime.TryParseExact(s, format, CultureInfo.CurrentUICulture, DateTimeStyles.None,
                            out var d1))
                    {
                        return _clock.Normalize(d1);
                    }
                }
            }
            else
            {
                throw new JsonException("读取器的TokenType不是String！");
            }
        }

        if (reader.TryGetDateTime(out var d2))
        {
            return _clock.Normalize(d2);
        }

        throw new JsonException("无法从读取器中获取日期时间！");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        if (_serializerOptions.OutputDateTimeFormat.IsNullOrWhiteSpace())
        {
            writer.WriteStringValue(_clock.Normalize(value));
        }
        else
        {
            writer.WriteStringValue(_clock.Normalize(value)
                .ToString(_serializerOptions.OutputDateTimeFormat, CultureInfo.CurrentUICulture));
        }
    }
}