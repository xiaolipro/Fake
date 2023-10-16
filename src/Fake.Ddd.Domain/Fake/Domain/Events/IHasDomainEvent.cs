namespace Fake.Domain.Events;

public interface IHasDomainEvent
{
    public IReadOnlyCollection<DomainEvent> DomainEvents { get; }
}