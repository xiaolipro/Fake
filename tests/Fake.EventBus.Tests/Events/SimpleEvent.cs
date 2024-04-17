using Fake.EventBus.Events;

namespace Fake.EventBus.Tests.Events;

public class SimpleEvent(int num) : EventBase
{
    public int Num { get; } = num;
}