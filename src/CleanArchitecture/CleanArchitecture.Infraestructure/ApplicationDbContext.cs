using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infraestructure;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    private readonly IPublisher _publisher;
    public ApplicationDbContext(DbContextOptions options, IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    /** 
    * OnModelCreating es un método que Entity Framework Core proporciona para personalizar y configurar el modelo de datos cuando se construye. 
    * Este método le dice a Entity Framework Core que busque y aplique todas las configuraciones de entidad (IEntityTypeConfiguration) 
    * que se encuentren en el ensamblado en el que está definida la clase ApplicationDbContext. 
    * Si el modelo crece y necesitas agregar más configuraciones, simplemente defines nuevas clases que implementen IEntityTypeConfiguration<T>.
    * No necesitas registrar manualmente cada configuración, ya que EF Core las detecta automáticamente.
    */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    // se encarga de guardar todos los cambios en la db
    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default
    )
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAsync(); 

        return result;
    }

    private async Task PublishDomainEventsAsync () 
    {
        try {
            var domainEvents = ChangeTracker
                .Entries<Entity>()
                .Select(entry => entry.Entity)
                .SelectMany(entity => {
                    var domainEvents = entity.GetDomainEvents();
                    entity.ClearDomainEvents();
                    return domainEvents; 
                })
            ;

            foreach (var domainEvent in domainEvents) 
            {
                await _publisher.Publish(domainEvent);
            };
        } catch (DbUpdateConcurrencyException ex)
        {
            /**
            * La excepción de concurrencia en programación, como 'DbUpdateConcurrencyException' en Entity Framework, 
            * se utiliza para manejar situaciones en las que varias operaciones intentan modificar el mismo conjunto de datos al mismo tiempo, y esto causa un conflicto.
            * Conflictos
            */

            // Si se detecta que hay un problema a nivel de base de datos al momento de la inserción se dispara
            throw new ConcurrencyException("Concurrency exception thrown.", ex); 
        }
    }
}