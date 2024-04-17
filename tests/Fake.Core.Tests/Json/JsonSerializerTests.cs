using System.Text.Json;
using System.Text.Json.Serialization;
using Fake.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Core.Tests.Json;

public class JsonSerializerTests : JsonTestBase
{
    private readonly IFakeJsonSerializer _jsonSerializer;

    public JsonSerializerTests()
    {
        _jsonSerializer = ServiceProvider.GetRequiredService<IFakeJsonSerializer>();
    }

    protected override void AfterAddFakeApplication(IServiceCollection services)
    {
        services.Configure<JsonSerializerOptions>(_ =>
        {
            //options.Encoder = JavaScriptEncoder.Default;
        });
    }


    [Fact]
    void 序列化()
    {
        var student = new Student
        {
            Id = 19749823749832759,
            Name = "张三",
            Is18 = true
        };

        var json = _jsonSerializer.Serialize(student);

        json.ShouldBe("{\"id\":19749823749832759,\"name\":\"张三\",\"is18\":true}");
    }

    [Fact]
    void 反序列化()
    {
        var json = "{\"id\":19749823749832759,\"name\":\"张三\",\"is18\":true}";

        var student = _jsonSerializer.Deserialize<Student>(json);

        student.Name.ShouldBe("张三");
        student.Is18.ShouldBe(true);
        student.Id.ShouldBe(19749823749832759);
    }

    [Fact]
    void JsonIgnore序列化()
    {
        var student = new Student
        {
            Id = 19749823749832759,
            Name = "张三",
            Is18 = true,
            IgnoreMember = "IgnoreMember"
        };

        var json = _jsonSerializer.Serialize(student);

        json.ShouldBe("{\"id\":19749823749832759,\"name\":\"张三\",\"is18\":true}");
    }

    [Fact]
    void JsonIgnore反序列化()
    {
        var json = "{\"id\":19749823749832759,\"name\":\"张三\",\"is18\":true,\"ignoreMember\":\"IgnoreMember\"}";

        var student = _jsonSerializer.Deserialize<Student>(json);

        student.Name.ShouldBe("张三");
        student.Is18.ShouldBe(true);
        student.Id.ShouldBe(19749823749832759);
        student.IgnoreMember.ShouldBeNull();
    }

    [Fact]
    void JsonPropertyName序列化()
    {
        var student = new NewStudent
        {
            Id = 19749823749832759,
            Name = "张三",
            Is18 = true,
            IgnoreMember = "IgnoreMember",
            Sex = "男"
        };

        var json = _jsonSerializer.Serialize(student);

        json.ShouldNotBe("{\"id\":19749823749832759,\"name\":\"张三\",\"is18\":true,\"gender\":\"男\"}");
        json.ShouldBe("{\"gender\":\"男\",\"id\":19749823749832759,\"name\":\"张三\",\"is18\":true}");
    }

    [Fact]
    void JsonPropertyName反序列化()
    {
        var json =
            "{\"id\":19749823749832759,\"name\":\"张三\",\"is18\":true,\"ignoreMember\":\"IgnoreMember\",\"gender\":\"男\"}";

        var student = _jsonSerializer.Deserialize<NewStudent>(json);

        student.Name.ShouldBe("张三");
        student.Is18.ShouldBe(true);
        student.Id.ShouldBe(19749823749832759);
        student.IgnoreMember.ShouldBeNull();
        student.Sex.ShouldBe("男");
    }

    class Student
    {
        public long? Id { get; set; }

        public string? Name { get; set; }

        public bool Is18 { get; set; }

        [JsonIgnore] public string? IgnoreMember { get; set; }
    }


    class NewStudent : Student
    {
        [JsonPropertyName("gender")] public string? Sex { get; set; }
    }
}