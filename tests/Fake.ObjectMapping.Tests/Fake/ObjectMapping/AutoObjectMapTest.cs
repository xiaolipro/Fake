using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Fake.ObjectMapping;

public abstract class AutoObjectMapTest:FakeObjectMappingTestBase
{
    private readonly IObjectMapper _objectMapper;

    public AutoObjectMapTest()
    {
        _objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();
    }
    
    [Fact]
    void 自动映射()
    {
        var dto =_objectMapper.Map<MyEntity, MyDto>(new MyEntity { Number = 42 });
        dto.Number.ShouldBe(42);
    }
}


public class MyEntity
{
    public Guid Id { get; set; }

    public int Number { get; set; }
}

public class MyDto
{
    public Guid Id { get; set; }

    public int Number { get; set; }
}

