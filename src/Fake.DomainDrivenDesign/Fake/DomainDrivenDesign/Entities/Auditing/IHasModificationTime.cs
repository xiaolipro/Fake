namespace Fake.DomainDrivenDesign.Entities.Auditing;

public interface IHasModificationTime
{
    /// <summary>
    /// 上一次修改时间
    /// </summary>
    DateTime? LastModificationTime { get; }
}