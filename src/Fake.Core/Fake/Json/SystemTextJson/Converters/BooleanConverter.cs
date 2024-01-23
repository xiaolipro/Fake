using System.Buffers;
using System.Buffers.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace Fake.Json.SystemTextJson.Converters;

public class BooleanConverter : JsonConverter<bool>
{
    private readonly FakeJsonSerializerOptions _options;

    public BooleanConverter(IOptions<FakeJsonSerializerOptions> options)
    {
        _options = options.Value;
    }

    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String && _options.StringToBoolean)
        {
            var span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;

            // 将utf8编码的字节序列转换为bool值，这是一个高性能，低分配的方法
            if (Utf8Parser.TryParse(span, out bool b1, out var bytesConsumed) && span.Length == bytesConsumed)
            {
                return b1;
            }

            if (bool.TryParse(reader.GetString(), out var b2))
            {
                return b2;
            }
        }

        return reader.GetBoolean();
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value);
    }
}