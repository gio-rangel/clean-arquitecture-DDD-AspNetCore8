using CleanArchitecture.Domain.Cars.Entities;

namespace CleanArchitecture.Domain.Cars.Interfaces;

public interface ICarRepository 
{
    Task<Car?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default); 
}