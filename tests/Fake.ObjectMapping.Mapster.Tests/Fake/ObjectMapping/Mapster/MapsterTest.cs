using Fake.ObjectMapping.Models;
using Fake.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.ObjectMapping.Mapster;

public class MapsterTest : FakeIntegrationTest<FakeObjectMappingMapsterTestModule>
{
    private readonly IObjectMapper _objectMapper;
    public MapsterTest() => _objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();

    [Fact]
    void 使用Mapster作为ObjectMapper供应商()
    {
        var provider = ServiceProvider.GetRequiredService<IObjectMappingProvider>();
        Assert.True(provider is MapsterObjectMappingProvider);
    }

    [Fact]
    void 映射()
    {
        var dto = _objectMapper.Map<MyEntity, MyDto>(new MyEntity { Number = 42 });
        Assert.Equal(42, dto.Number);

        // ReverseMap
        var entity = _objectMapper.Map<MyDto, MyEntity>(new MyDto { Number = 42 });
        Assert.Equal(42, entity.Number);
    }
}