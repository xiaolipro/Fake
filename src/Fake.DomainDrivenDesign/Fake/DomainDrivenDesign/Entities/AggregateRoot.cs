using Fake.DomainDrivenDesign.Entities.Auditing;
using Fake.IdGenerators.GuidGenerator;

namespace Fake.DomainDrivenDesign.Entities;

[Serializable]
public abstract class AggregateRoot : Entity, IAggregateRoot, IHasVersionNum
{
    public string VersionNum { get; set; }

    protected AggregateRoot()
    {
        VersionNum = SimpleGuidGenerator.Instance.GenerateAsString();
    }
}

[Serializable]
public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>, IHasVersionNum
{
    public string VersionNum { get; set; }

    protected AggregateRoot()
    {
        VersionNum = SimpleGuidGenerator.Instance.GenerateAsString();
    }
}