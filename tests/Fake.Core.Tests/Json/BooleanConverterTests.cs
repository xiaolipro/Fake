using Fake.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Core.Tests.Json;

public class BooleanConverterTests : JsonTestBase
{
    private readonly IFakeJsonSerializer _jsonSerializer;

    public BooleanConverterTests()
    {
        _jsonSerializer = ServiceProvider.GetRequiredService<IFakeJsonSerializer>();
    }

    protected override void AfterAddFakeApplication(IServiceCollection services)
    {
        services.Configure<FakeJsonSerializerOptions>(options => { options.StringToBoolean = true; });
    }


    [Fact]
    void 序列化()
    {
        var student = new Student
        {
            Name = "张三",
            Is18 = true
        };

        var json = _jsonSerializer.Serialize(student);

        json.ShouldBe("{\"name\":\"张三\",\"is18\":true}");
    }

    [Fact]
    void 反序列化()
    {
        var json = "{\"Name\":\"张三\",\"Is18\":\"true\"}";

        var student = _jsonSerializer.Deserialize<Student>(json);

        student.Name.ShouldBe("张三");
        student.Is18.ShouldBe(true);

        json = "{\"Name\":\"张三\",\"Is18\":false}";

        student = _jsonSerializer.Deserialize<Student>(json);

        student.Name.ShouldBe("张三");
        student.Is18.ShouldBe(false);
    }

    [Fact]
    void 不区分大小写的()
    {
        var json = "{\"Name\":\"张三\",\"Is18\":\"trUe\"}";

        var student = _jsonSerializer.Deserialize<Student>(json);

        student.Name.ShouldBe("张三");
        student.Is18.ShouldBe(true);

        json = "{\"Name\":\"张三\",\"Is18\":\"FalSe\"}";

        student = _jsonSerializer.Deserialize<Student>(json);

        student.Name.ShouldBe("张三");
        student.Is18.ShouldBe(false);
    }

    [Fact]
    void 可空bool反序列化()
    {
        var json = "{\"Value\":\"true\"}";

        var student = _jsonSerializer.Deserialize<NullableBooleanClass>(json);

        student.Value.ShouldBe(true);

        json = "{\"Value\":\"false\"}";

        student = _jsonSerializer.Deserialize<NullableBooleanClass>(json);

        student.Value.ShouldBe(false);

        json = "{\"Value\":null}";

        student = _jsonSerializer.Deserialize<NullableBooleanClass>(json);

        student.Value.ShouldBeNull();
    }

    [Fact]
    void 可空bool序列化()
    {
        var nullableBooleanClass = new NullableBooleanClass
        {
            Value = true
        };

        var json = _jsonSerializer.Serialize(nullableBooleanClass);

        json.ShouldBe("{\"value\":true}");

        nullableBooleanClass = new NullableBooleanClass
        {
            Value = false
        };

        json = _jsonSerializer.Serialize(nullableBooleanClass);

        json.ShouldBe("{\"value\":false}");

        nullableBooleanClass = new NullableBooleanClass
        {
            Value = null
        };

        json = _jsonSerializer.Serialize(nullableBooleanClass);

        json.ShouldBe("{\"value\":null}");
    }

    class Student
    {
        public string? Name { get; set; }

        public bool Is18 { get; set; }
    }

    class NullableBooleanClass
    {
        public bool? Value { get; set; }
    }
}