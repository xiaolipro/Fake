using Fake.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Core.Tests.Json;

public class LongConverterTests : JsonTestBase
{
    private readonly IFakeJsonSerializer _jsonSerializer;

    public LongConverterTests()
    {
        _jsonSerializer = ServiceProvider.GetRequiredService<IFakeJsonSerializer>();
    }

    protected override void AfterAddFakeApplication(IServiceCollection services)
    {
        services.Configure<FakeJsonSerializerOptions>(options => { options.LongToString = true; });
    }


    [Fact]
    void 序列化()
    {
        long id = 1293829749328751111;
        var student = new Student
        {
            Id = id,
            Name = "张三",
            Is18 = true
        };

        var json = _jsonSerializer.Serialize(student);

        json.ShouldBe("{\"id\":\"1293829749328751111\",\"name\":\"张三\",\"is18\":true}");
    }

    [Fact]
    void 反序列化()
    {
        var json = "{\"id\":\"1293829749328751111\",\"name\":\"张三\",\"is18\":true}";

        var student = _jsonSerializer.Deserialize<Student>(json);

        student.Name.ShouldBe("张三");
        student.Is18.ShouldBe(true);
        student.Id.ShouldBe(1293829749328751111);
    }

    [Fact]
    void 可空long序列化()
    {
        var nullableLongClass = new NullableLongClass
        {
            Value = 1293829749328751111
        };

        var json = _jsonSerializer.Serialize(nullableLongClass);

        json.ShouldBe("{\"value\":\"1293829749328751111\"}");

        nullableLongClass = new NullableLongClass
        {
            Value = null
        };

        json = _jsonSerializer.Serialize(nullableLongClass);

        json.ShouldBe("{\"value\":null}");
    }

    [Fact]
    void 可空long反序列化()
    {
        var json = "{\"value\":\"1293829749328751111\"}";

        var nullableLongClass = _jsonSerializer.Deserialize<NullableLongClass>(json);

        nullableLongClass.Value.ShouldBe(1293829749328751111);

        json = "{\"value\":null}";

        nullableLongClass = _jsonSerializer.Deserialize<NullableLongClass>(json);

        nullableLongClass.Value.ShouldBeNull();
    }

    class Student
    {
        public long? Id { get; set; }

        public string? Name { get; set; }

        public bool Is18 { get; set; }
    }

    class NullableLongClass
    {
        public long? Value { get; set; }
    }
}