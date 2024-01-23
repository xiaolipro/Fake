using System.Buffers;
using System.Buffers.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace Fake.Json.SystemTextJson.Converters;

public class LongConverter : JsonConverter<long>
{
    private readonly FakeJsonSerializerOptions _options;

    public LongConverter(IOptions<FakeJsonSerializerOptions> options)
    {
        _options = options.Value;
    }

    public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;

            // 将utf8编码的字节序列转换为long值，这是一个高性能，低分配的方法
            if (Utf8Parser.TryParse(span, out long l1, out var bytesConsumed) && span.Length == bytesConsumed)
            {
                return l1;
            }

            if (long.TryParse(reader.GetString(), out var l2))
            {
                return l2;
            }
        }

        return reader.GetInt64();
    }

    public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
    {
        if (_options.LongToString)
        {
            // js number类型的最大值为2^53-1，超过这个值会丢失精度
            writer.WriteStringValue(value.ToString());
        }
        else
        {
            writer.WriteNumberValue(value);
        }
    }
}