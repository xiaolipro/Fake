namespace Fake.Domain.Entities.Auditing;

public abstract class FullAuditedAggregate<TKey> : AggregateRoot<TKey>, IFullAuditedEntity<TKey>
{
    public TKey CreatorId { get; }
    public TKey LastModifierId { get; }
    public DateTime CreationTime { get; }
    public DateTime? LastModificationTime { get; }
    public bool IsDeleted { get; }
    public bool HardDeleted { get; }
}