
using Fake.Domain.Entities.Auditing;
using Fake.Domain.Entities.IdGenerators;

namespace Fake.Domain.Entities;


[Serializable]
public abstract class AggregateRoot : Entity, IAggregateRoot,IHasVersionNum
{
    public string VersionNum { get; set; }

    protected AggregateRoot()
    {
        VersionNum = SimpleGuidGenerator.Instance.Create().ToString("N");
    }
}

[Serializable]
public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
{
    public Guid Version { get; set; }

    protected AggregateRoot()
    {
        Version = Guid.NewGuid();
    }
}