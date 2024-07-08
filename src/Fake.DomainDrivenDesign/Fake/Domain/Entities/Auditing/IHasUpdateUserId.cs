namespace Fake.Domain.Entities.Auditing;

public interface IHasUpdateUserId
{
    /*
     * 在设计上，希望规避可空值类型
     */

    /// <summary>
    /// 更新用户Id
    /// </summary>
    Guid UpdateUserId { get; }
}