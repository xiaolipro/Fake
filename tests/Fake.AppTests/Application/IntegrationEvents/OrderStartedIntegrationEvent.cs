﻿using Fake.Domain.Events;

namespace Application.IntegrationEvents;

public class OrderStartedIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; set; }

    public OrderStartedIntegrationEvent(Guid userId)
        => UserId = userId;
}