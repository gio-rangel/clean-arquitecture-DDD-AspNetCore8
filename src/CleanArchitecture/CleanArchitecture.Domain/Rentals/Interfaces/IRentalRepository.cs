using CleanArchitecture.Domain.Cars.Entities;
using CleanArchitecture.Domain.Rentals.Entities;
using CleanArchitecture.Domain.Rentals.ValueObjects;

namespace CleanArchitecture.Domain.Rentals.Interfaces;

public interface IRentalRepository 
{
    Task<Rental?> GetRentalByIdAsync (Guid id, CancellationToken cancellationToken= default);

    Task<bool> IsOverlappingAsync (Car car, DateRange rentalPeriod, CancellationToken cancellationToken = default);            

    void Add(Rental rental);                 
}