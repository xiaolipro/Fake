using Fake.DependencyInjection;
using Fake.ObjectMapping.Models.Entities;

namespace Fake.ObjectMapping.SpecificMapper;

public class MyEntity2SimpleEntityMapper: IObjectMapper<MyEntity, SimpleEntity>, ITransientDependency
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