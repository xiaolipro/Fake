using System;

namespace Fake.Domain.Entities.Auditing;

public interface IHasModifier<out TUserId>
{
    /// <summary>
    /// 上一次修改者Id
    /// </summary>
    TUserId LastModifierId { get; }
    
    /// <summary>
    /// 上一次修改时间
    /// </summary>
    DateTime LastModificationTime { get; }
}