using Fake.DomainDrivenDesign.Events;

namespace Fake.DomainDrivenDesign.Entities;

[Serializable]
public abstract class BasicAggregateRoot : Entity, IAggregateRoot, IHasDomainEvent
{
    private List<DomainEvent>? _domainEvents;

    public IReadOnlyCollection<DomainEvent>? DomainEvents => _domainEvents?.AsReadOnly();

    protected virtual void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents ??= new List<DomainEvent>();
        _domainEvents.Add(domainEvent);
    }

    protected virtual void RemoveDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents?.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
}

[Serializable]
public abstract class BasicAggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>, IHasDomainEvent
{
    private List<DomainEvent>? _domainEvents;

    public IReadOnlyCollection<DomainEvent>? DomainEvents => _domainEvents?.AsReadOnly();

    protected virtual void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents ??= new List<DomainEvent>();
        _domainEvents.Add(domainEvent);
    }

    protected virtual void RemoveDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents?.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
}