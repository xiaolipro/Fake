﻿using Fake.EventBus.Events;

namespace Fake.DomainDrivenDesign.Events;

/// <summary>
/// 领域事件
/// </summary>
public class DomainEvent : EventBase
{
    public override string ToString()
    {
        return $"[领域事件：{GetType().Name} Id：{Id} 创建时间：{CreationTime}]";
    }
}