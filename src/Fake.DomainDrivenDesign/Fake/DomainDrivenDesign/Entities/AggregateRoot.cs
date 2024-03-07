using Fake.Auditing;
using Fake.DomainDrivenDesign.Entities.Auditing;
using Fake.IdGenerators.GuidGenerator;

namespace Fake.DomainDrivenDesign.Entities;

[Serializable]
public abstract class AggregateRoot : BasicAggregateRoot, IHasVersionNum
{
    [DisableAuditing] public virtual string VersionNum { get; set; }

    protected AggregateRoot()
    {
        VersionNum = SimpleGuidGenerator.Instance.GenerateAsString();
    }
}

[Serializable]
public abstract class AggregateRoot<TKey> : BasicAggregateRoot<TKey>, IHasVersionNum
{
    [DisableAuditing] public string VersionNum { get; set; } = SimpleGuidGenerator.Instance.GenerateAsString();
}