﻿using Fake.EventBus.Events;

namespace Fake.DomainDrivenDesign.Events;

/// <summary>
/// 集成事件
/// </summary>
public class IntegrationEvent : EventBase
{
    public override string ToString()
    {
        return $"[集成事件：{GetType().Name} Id：{Id} 创建时间：{CreationTime}]";
    }
}