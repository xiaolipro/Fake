using System.ComponentModel.DataAnnotations.Schema;
using Fake.Domain.Entities.Events;

namespace Fake.Domain.Entities;

public class HasDomainEvent : IHasDomainEvent
{
    private List<DomainEvent> _domainEvents;

    [NotMapped] 
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents ??= new List<DomainEvent>();
        _domainEvents.Add(domainEvent);
    }

    protected void RemoveDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents?.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
}