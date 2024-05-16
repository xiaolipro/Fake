using Fake.DependencyInjection;
using Fake.ObjectMapping.Tests.Models;

namespace Fake.ObjectMapping.Tests.SpecificMapper;

public class MyEntity2SimpleEntityMapper : IObjectMapper<MyEntity, SimpleEntity>, ITransientDependency
{
    public SimpleEntity Map(MyEntity source)
    {
        return new SimpleEntity
        {
            Id = source.Id,
            Number = source.Number + 1
        };
    }

    public SimpleEntity Map(MyEntity source, SimpleEntity destination)
    {
        destination.Id = source.Id;
        destination.Number = source.Number + 1;
        return destination;
    }
}