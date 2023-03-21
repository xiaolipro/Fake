namespace Fake.Domain.Entities.Auditing;

public abstract class FullAuditedAggregate<TKey, TUserId> : AggregateRoot<TKey>, IFullAuditedEntity<TUserId>
{
    public TUserId CreatorId { get; }
    public TUserId LastModifierId { get; }
    public DateTime CreationTime { get; }
    public DateTime LastModificationTime { get; }
    public bool IsDeleted { get; }
    public bool HardDeleted { get; }
}