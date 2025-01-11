using CleanArchitecture.Domain.Cars.Entities;
using CleanArchitecture.Domain.Rentals.Entities;
using CleanArchitecture.Domain.Rentals.Enums;
using CleanArchitecture.Domain.Rentals.Interfaces;
using CleanArchitecture.Domain.Rentals.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories;

internal sealed class RentalRepository : Repository<Rental>, IRentalRepository
{
    private static readonly RentalStatus[] ActiveRentalStatuses = {
        RentalStatus.Booked,
        RentalStatus.Confirmed,
        RentalStatus.Confirmed
    };

    public RentalRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> IsOverlappingAsync(Car car, DateRange rentalPeriod, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Rental>()
            .AnyAsync(rental => 
                rental.CarId == car.Id &&
                rental.RentalPeriod.Start <= rentalPeriod.End &&
                rental.RentalPeriod.End >= rentalPeriod.Start &&
                ActiveRentalStatuses.Contains(rental.Status),
                cancellationToken
            )
        ; 
    }
}
