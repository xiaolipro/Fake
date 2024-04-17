using Fake.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Core.Tests.Json;

public class DateTimeConverterTests : JsonTestBase
{
    private readonly IFakeJsonSerializer _jsonSerializer;

    public DateTimeConverterTests()
    {
        _jsonSerializer = ServiceProvider.GetRequiredService<IFakeJsonSerializer>();
    }

    protected override void AfterAddFakeApplication(IServiceCollection services)
    {
        services.Configure<FakeJsonSerializerOptions>(options =>
        {
            options.InputDateTimeFormats.Add("yyyy-MM-dd HH:mm:ss");
            options.OutputDateTimeFormat = "yyyy-MM-dd HH:mm";
        });
    }

    [Fact]
    void 序列化()
    {
        var datetime = new DatetimeClass
        {
            Value = new DateTime(2021, 1, 1, 1, 1, 1)
        };

        var json = _jsonSerializer.Serialize(datetime);

        json.ShouldBe("{\"value\":\"2021-01-01 01:01\"}");
    }


    [Fact]
    void 反序列化()
    {
        var json = "{\"value\":\"2021-01-01 01:01:01\"}";

        var datetime = _jsonSerializer.Deserialize<DatetimeClass>(json);

        datetime.Value.ShouldBe(new DateTime(2021, 1, 1, 1, 1, 1));

        json = "{\"value\":\"2021-01-01T01:01:01\"}";

        datetime = _jsonSerializer.Deserialize<DatetimeClass>(json);

        datetime.Value.ShouldBe(new DateTime(2021, 1, 1, 1, 1, 1));
    }

    [Fact]
    void 可空datetime反序列化()
    {
        var json = "{\"value\":\"2021-01-01 01:01:01\"}";

        var datetime = _jsonSerializer.Deserialize<NullableDatetimeClass>(json);

        datetime.Value.ShouldBe(new DateTime(2021, 1, 1, 1, 1, 1));

        json = "{\"value\":\"2021-01-01T01:01:01\"}";

        datetime = _jsonSerializer.Deserialize<NullableDatetimeClass>(json);

        datetime.Value.ShouldBe(new DateTime(2021, 1, 1, 1, 1, 1));

        json = "{\"value\":null}";

        datetime = _jsonSerializer.Deserialize<NullableDatetimeClass>(json);

        datetime.Value.ShouldBeNull();
    }

    [Fact]
    void 可空datetime序列化()
    {
        var datetime = new NullableDatetimeClass
        {
            Value = new DateTime(2021, 1, 1, 1, 1, 1)
        };

        var json = _jsonSerializer.Serialize(datetime);

        json.ShouldBe("{\"value\":\"2021-01-01 01:01\"}");

        datetime = new NullableDatetimeClass
        {
            Value = new DateTime(2021, 1, 1, 1, 1, 1)
        };

        json = _jsonSerializer.Serialize(datetime);

        json.ShouldBe("{\"value\":\"2021-01-01 01:01\"}");

        datetime = new NullableDatetimeClass
        {
            Value = null
        };

        json = _jsonSerializer.Serialize(datetime);

        json.ShouldBe("{\"value\":null}");
    }


    class DatetimeClass
    {
        public DateTime Value { get; set; }
    }


    class NullableDatetimeClass
    {
        public DateTime? Value { get; set; }
    }
}