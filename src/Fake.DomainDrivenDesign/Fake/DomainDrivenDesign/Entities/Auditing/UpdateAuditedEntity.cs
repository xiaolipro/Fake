namespace Fake.DomainDrivenDesign.Entities.Auditing;

/// <summary>
/// 更新审计实体
/// </summary>
/// <typeparam name="TKey">id类型</typeparam>
[Serializable]
public abstract class UpdateAuditedEntity<TKey> : Entity<TKey>, IHasUpdateUserId, IHasUpdateTime
{
    public virtual Guid? UpdateUserId { get; set; }
    public virtual DateTime? UpdateTime { get; set; }
}

/// <summary>
/// 更新审计实体
/// </summary>
/// <typeparam name="TKey">id类型</typeparam>
[Serializable]
public abstract class SoftDeleteAuditedEntity<TKey> : Entity<TKey>, ISoftDelete
{
    public virtual bool IsDeleted { get; set; }
    public bool HardDeleted { get; } = false;
}