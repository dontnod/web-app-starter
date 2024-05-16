namespace WebAppStarter.Domain.Common;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public abstract class BaseEntity
{
    private readonly List<BaseEvent> domainEvents = new();

    // This can easily be modified to be BaseEntity<T> and public T Id to support different key types.
    // Using non-generic integer types for simplicity
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        domainEvents.Clear();
    }
}
