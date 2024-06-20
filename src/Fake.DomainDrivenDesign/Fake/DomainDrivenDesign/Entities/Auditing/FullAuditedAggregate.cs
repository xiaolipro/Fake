namespace Fake.DomainDrivenDesign.Entities.Auditing;

[Serializable]
public abstract class FullAuditedAggregate<TKey> : AggregateRoot<TKey>, IFullAuditedEntity
{
    public virtual Guid CreateUserId { get; set; }
    public virtual DateTime CreateTime { get; set; }
    public virtual Guid UpdateUserId { get; set; }
    public virtual DateTime UpdateTime { get; set; }
    public virtual bool IsDeleted { get; set; }
}