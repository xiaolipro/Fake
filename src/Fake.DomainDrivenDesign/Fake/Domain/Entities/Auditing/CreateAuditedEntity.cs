namespace Fake.Domain.Entities.Auditing;

/// <summary>
/// 创建审计实体
/// </summary>
/// <typeparam name="TKey"></typeparam>
[Serializable]
public abstract class CreateAuditedEntity<TKey> : Entity<TKey>, IHasCreateUserId, IHasCreateTime
{
    public virtual Guid CreateUserId { get; set; }
    public virtual DateTime CreateTime { get; set; }
}