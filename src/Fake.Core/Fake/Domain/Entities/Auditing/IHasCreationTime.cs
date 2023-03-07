namespace Fake.Domain.Entities.Auditing;

public interface IHasCreationTime
{
    /// <summary>
    /// 创建时间
    /// </summary>
    DateTime CreationTime { get; }
}