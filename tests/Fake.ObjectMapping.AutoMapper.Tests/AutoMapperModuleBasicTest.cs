using Fake.ObjectMapping.Tests.Models;
using Fake.Testing;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Fake.ObjectMapping.AutoMapper.Tests;

public class AutoMapperModuleBasicTest : ApplicationTest<FakeObjectMappingAutoMapperTestModule>
{
    private readonly IObjectMapper _objectMapper;

    public AutoMapperModuleBasicTest()
    {
        _objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();
    }

    [Fact]
    void 使用AutoMapper作为ObjectMapper供应商()
    {
        var provider = ServiceProvider.GetRequiredService<IObjectMappingProvider>();
        (provider is AutoMapperObjectMappingProvider).ShouldBe(true);
    }

    [Fact]
    void 映射()
    {
        var dto = _objectMapper.Map<MyEntity, MyDto>(new MyEntity { Number = 42 });
        dto.Number.ShouldBe(42);

        // ReverseMap
        var entity = _objectMapper.Map<MyDto, MyEntity>(new MyDto { Number = 42 });
        entity.Number.ShouldBe(42);
    }

    [Fact]
    void 不传目标实例则创建新对象()
    {
        var entity = new MyEntity() { Id = Guid.NewGuid(), Number = 1 };
        var res = _objectMapper.Map<MyEntity, MyDto>(entity);
        res.Id.ShouldBe(entity.Id);
        res.Number.ShouldBe(1);
    }


    [Fact]
    void 传目标实例则不创建新对象()
    {
        var entity = new MyEntity() { Id = Guid.NewGuid(), Number = 1 };
        var simpleEntity = new MyDto();
        var res = _objectMapper.Map(entity, simpleEntity);
        res.ShouldBe(simpleEntity);
        res.Id.ShouldBe(entity.Id);
        simpleEntity.Id.ShouldBe(entity.Id);
        res.Number.ShouldBe(1);
    }
}