namespace Fake.Domain.Entities.Auditing;

public interface IHasUpdateTime
{
    /// <summary>
    /// 更新时间
    /// </summary>
    DateTime UpdateTime { get; }
}