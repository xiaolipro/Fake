namespace Fake.Domain.Entities.Auditing;

public interface IHasVersionNum
{
    /// <summary>
    /// 版本号
    /// </summary>
    string VersionNum { get; set; }
}