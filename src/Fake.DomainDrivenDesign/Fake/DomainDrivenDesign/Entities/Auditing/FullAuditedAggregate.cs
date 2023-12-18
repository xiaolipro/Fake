#nullable enable
namespace Fake.DomainDrivenDesign.Entities.Auditing;

public abstract class FullAuditedAggregate<TKey, TUserId> : AggregateRoot<TKey>, IFullAuditedEntity<TUserId>
{
    public TUserId? CreatorId { get; set; }
    public DateTime CreationTime { get; set; }
    public TUserId? LastModifierId { get; set; }
    public DateTime? LastModificationTime { get; set; }
    public bool IsDeleted { get; set; }
    public bool HardDeleted { get; set; }
}