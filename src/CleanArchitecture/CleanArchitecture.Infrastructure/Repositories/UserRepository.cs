using CleanArchitecture.Domain.Users.Entities;
using CleanArchitecture.Domain.Users.Interfaces;

namespace CleanArchitecture.Infrastructure.Repositories;

/**
* Al heredar la clase generica abstracta Repository se implementa la logica que injecta el dbContext 
* Ademas de lógica general que compete a todas las entidades
* IUserRepository implementa todos los metodos personalizados para User por lo que no hace falta volver a definirlo
*/
internal sealed class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {

    }
}