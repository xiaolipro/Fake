namespace Fake.Domain.Entities.Auditing;

public abstract class FullAuditedAggregate<TKey, TAuditor> : AggregateRoot<TKey>, IFullAuditedEntity<TAuditor>
{
    public TAuditor CreatorId { get; }
    public TAuditor LastModifierId { get; }
    public DateTime CreationTime { get; }
    public DateTime? LastModificationTime { get; }
    public bool IsDeleted { get; }
    public bool HardDeleted { get; }
}