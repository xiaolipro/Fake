﻿using System;
using Fake.EventBus.Events;

namespace Application.IntegrationEvents;

public class OrderStartedIntegrationEvent: IntegrationEvent
{
    public Guid UserId { get; set; }

    public OrderStartedIntegrationEvent(Guid userId)
        => UserId = userId;
}