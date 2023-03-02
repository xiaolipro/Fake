using System;

namespace Fake.Domain.Entities.Auditing;

public interface IHasCreator<out TUserId>
{
    /// <summary>
    /// 创建者Id
    /// </summary>
    TUserId CreatorId { get; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    DateTime CreationTime { get; }
}