#nullable enable
using System;

namespace Fake.Domain.Entities.Auditing;

public interface IHasDeleter<out TUserId>
{
    /// <summary>
    /// 删除者Id
    /// </summary>
    TUserId? DeleterId { get; }
    
    /// <summary>
    /// 删除时间
    /// </summary>
    DateTime? DeletionTime { get; }
}