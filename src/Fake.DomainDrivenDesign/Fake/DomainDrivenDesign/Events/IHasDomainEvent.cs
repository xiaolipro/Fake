namespace Fake.DomainDrivenDesign.Events;

public interface IHasDomainEvent
{
    public IReadOnlyCollection<DomainEvent>? DomainEvents { get; }
}