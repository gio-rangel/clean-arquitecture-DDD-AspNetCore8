using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Users.Events;

namespace CleanArchitecture.Domain.Users.Entities;

public sealed class User: Entity 
{
    private User(
        Guid id,
        FirstName firstName,
        LastName lastName,
        Email email
    ) : base(id)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName; 
        Email = email;
    }

    public FirstName? FirstName { get; private set; }
    public LastName? LastName { get; private set; }
    public Email Email { get; private set; }

    public static User Create(
        FirstName firstName,
        LastName lastName,
        Email email
    )
    {
        var user = new User(Guid.NewGuid(), firstName, lastName, email);
        
        /**
        * Esto nos permite que cada vez que creemos un usuario 
        * gatillar un evento que ejecute logica de negocio...
        */
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

        return user;
    }
}