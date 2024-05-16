using Fake.Helpers;
using Fake.ObjectMapping.Tests.Models;
using Fake.Testing;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Fake.ObjectMapping.AutoMapper.Tests;

public class AutoMapperValidateTest : ApplicationTest<FakeObjectMappingAutoMapperTestModule>
{
    private readonly IObjectMapper _objectMapper;

    public AutoMapperValidateTest()
    {
        _objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();
    }

    [Fact]
    void 如果不开校验_则无法映射的属性跳过赋值()
    {
        // Arrange 
        var entity = new SimpleEntity
        {
            Id = Guid.NewGuid(),
            Number = RandomHelper.Next()
        };
        // Act
        var dto = _objectMapper.Map<SimpleEntity, MyMoreNoValidateDto>(entity);
        // Assert
        dto.Id.ShouldBe(entity.Id);
        dto.Number.ShouldBe(entity.Number);
        dto.UpdateTime.ShouldBe(default);
    }

    [Fact]
    void 如果开校验_则映射目标的属性必须全部能够映射()
    {
        // Arrange 
        var entity = new SimpleEntity
        {
            Id = Guid.NewGuid(),
            Number = RandomHelper.Next()
        };
        // Act
        _objectMapper.Map<SimpleEntity, MyMoreValidateDto>(entity);
    }
}

public class MyMoreValidateDto : MyDto
{
}

public class MyMoreNoValidateDto : MyDto
{
    public DateTime UpdateTime { get; set; }
}