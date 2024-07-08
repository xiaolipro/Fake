namespace Fake.Domain.Entities;

[Serializable]
public abstract class Entity : IEntity
{
    public abstract object[] GetKeys();

    public override string ToString()
    {
        return $"[实体: {GetType().Name}] Keys：{string.Join(", ", GetKeys())}";
    }

    public bool IsTransient => EntityHelper.IsTransientEntity(this);

    /// <summary>
    /// 实体相等性比较
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool EntityEquals(IEntity? other)
    {
        return EntityHelper.EntityEquals(this, other);
    }
}

/// <summary>
/// 实体抽象
/// </summary>
/// <typeparam name="TKey">实体唯一标识类型</typeparam>
[Serializable]
public abstract class Entity<TKey> : Entity, IEntity<TKey>
{
    /// <summary>
    /// 实体唯一标识
    /// </summary>
    public virtual TKey Id { get; private set; } = default!;

    protected Entity()
    {
    }

    protected Entity(TKey id)
    {
        Id = id;
    }

    public void SetId(TKey id) => Id = id;

    public override object[] GetKeys()
    {
        return new object[] { Id! };
    }

    public override string ToString()
    {
        return $"[实体: {GetType().Name}] Id：{Id}";
    }
}