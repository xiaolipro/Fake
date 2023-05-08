using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Json;

public class FakeJsonSerializerTests:FakeJsonTestBase
{
    private readonly IFakeJsonSerializer _jsonSerializer;

    public FakeJsonSerializerTests()
    {
        _jsonSerializer = ServiceProvider.GetRequiredService<IFakeJsonSerializer>();
    }

    protected override void AfterAddFakeApplication(IServiceCollection services)
    {
        services.Configure<JsonSerializerOptions>(options =>
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
    
    class Student
    {
        public long? Id { get; set; }
        
        public string Name { get; set; }

        public bool Is18 { get; set; }
    }
}