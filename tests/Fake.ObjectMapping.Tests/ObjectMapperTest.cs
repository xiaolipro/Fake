using Fake.ObjectMapping.Tests.Models;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit.Abstractions;

namespace Fake.ObjectMapping.Tests;

public class ObjectMapperTest : ObjectMappingTestBase
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IObjectMapper _objectMapper;

    public ObjectMapperTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();
    }

    [Fact]
    void 优先使用特定的映射器()
    {
        var entity = new MyEntity() { Id = Guid.NewGuid(), Number = 1 };
        var dto = _objectMapper.Map<MyEntity, SimpleEntity>(entity);
        dto.Number.ShouldBe(2);
    }

    [Fact]
    void 不传目标实例则创建新对象()
    {
        var entity = new MyEntity() { Id = Guid.NewGuid(), Number = 1 };
        var res = _objectMapper.Map<MyEntity, SimpleEntity>(entity);
        res.Id.ShouldBe(entity.Id);
        res.Number.ShouldBe(2);
    }


    [Fact]
    void 传目标实例则不创建新对象()
    {
        var entity = new MyEntity() { Id = Guid.NewGuid(), Number = 1 };
        var simpleEntity = new SimpleEntity();
        var res = _objectMapper.Map(entity, simpleEntity);
        res.ShouldBe(simpleEntity);
        res.Id.ShouldBe(entity.Id);
        simpleEntity.Id.ShouldBe(entity.Id);
        res.Number.ShouldBe(2);
    }
}