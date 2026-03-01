using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.Domain.Types;

namespace CleanArchitecture.Domain.Common;

public abstract class BaseAggregateRoot : IAggregateRoot, IAuditableEntity, IConcurrentEntity
{
    public Guid Id { get; set; }

    public Auditability Audit { get; set; } = null!;

    public Hex ConcurrencyToken { get; set; } = Array.Empty<byte>();

    private readonly List<BaseEvent> _domainEvents = [];

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
