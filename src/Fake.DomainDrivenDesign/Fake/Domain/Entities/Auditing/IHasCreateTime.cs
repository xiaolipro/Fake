namespace Fake.Domain.Entities.Auditing;

public interface IHasCreateTime
{
    /// <summary>
    /// 创建时间
    /// </summary>
    DateTime CreateTime { get; }
}