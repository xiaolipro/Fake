namespace Fake.DomainDrivenDesign.Entities.Auditing;

/// <summary>
/// 具有版本号，乐观锁
/// </summary>
public interface IHasVersionNum
{
    /// <summary>
    /// 版本号
    /// </summary>
    string VersionNum { get; set; }
}