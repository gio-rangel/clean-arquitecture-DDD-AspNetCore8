using CleanArchitecture.Domain.Cars.Entities;
using CleanArchitecture.Domain.Cars.Interfaces;

namespace CleanArchitecture.Infrastructure.Repositories;

internal sealed class CarRepository : Repository<Car>, ICarRepository
{
    public CarRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
