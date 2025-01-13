namespace CleanArchitecture.Domain.Abstractions;

public abstract class Entity 
{
    protected Entity() {} // Initiate a protected constructor for children entities;
    private readonly List<IDomainEvent> _domainEvents = new();
    protected Entity(Guid id) 
    {
        // Al ser 'protected' solo las clases hijas pueden inicializar el constructor.
        // Esto le da el sentido para que sea abstracta

        Id =  id; 
    }
    
    public Guid Id { get; init; } // init: no se puede mutar una vez definida

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.ToList();
    } 

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    } 

    protected void RaiseDomainEvent (IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}