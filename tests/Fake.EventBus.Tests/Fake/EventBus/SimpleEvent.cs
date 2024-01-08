using Fake.EventBus.Events;

namespace Fake.EventBus;

public class SimpleEvent(int num) : EventBase
{
    public int Num { get; } = num;
}