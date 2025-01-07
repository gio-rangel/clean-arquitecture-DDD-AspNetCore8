using CleanArchitecture.Domain.Users.Entities;

namespace CleanArchitecture.Domain.Users.Interfaces;

public interface IUserRepository 
{
    Task<User?> GetByIdAsync (Guid id, CancellationToken cancellationToken = default);

    void Add(User user);
}