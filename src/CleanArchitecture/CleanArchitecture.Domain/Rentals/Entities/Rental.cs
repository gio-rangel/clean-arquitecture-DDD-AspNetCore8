using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Shared.ValueObjects;
using CleanArchitecture.Domain.Rentals.ValueObjects;
using CleanArchitecture.Domain.Rentals.Events;
using CleanArchitecture.Domain.Rentals.Services;
using CleanArchitecture.Domain.Cars.Entities;
using CleanArchitecture.Domain.Rentals.Errors;
using CleanArchitecture.Domain.Rentals.Enums;

namespace CleanArchitecture.Domain.Rentals.Entities;

public sealed class Rental : Entity 
{ 
    private Rental() {}

    private Rental (
        Guid id,
        Guid carId,
        Guid userId,
        DateRange rentalPeriod,
        Currency basePrice,
        Currency maintenancePrice,
        Currency accesoriesPrice,
        Currency finalPrice,
        RentalStatus status,
        DateTime createdAt
    ) : base(id) 
    {
        Id = id;
        CarId = carId;
        UserId = userId;
        RentalPeriod = rentalPeriod;
        BasePrice = basePrice;
        MaintenancePrice = maintenancePrice;
        AccesoriesPrice = accesoriesPrice;
        FinalPrice = finalPrice;
        Status = status;
        CreatedAt = createdAt;
    }

    public Guid CarId { get; private set; }
    public Guid UserId { get; private set; }
    // BasePrice es el precio base (dias*precio de renta del auto)
    public Currency BasePrice { get; private set; }
    public Currency MaintenancePrice { get; private set; }
    public Currency AccesoriesPrice { get; private set; }
    public Currency FinalPrice { get; private set; }
    public RentalStatus Status { get; private set; }
    public DateRange RentalPeriod { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ConfirmationDate { get; private set; }
    public DateTime BookingRejectDate { get; private set; }
    public DateTime CompletationDate { get; private set; }
    public DateTime CancellationDate { get; private set; }

    public static Rental Book (
        Guid userId, 
        Car car, 
        DateRange rentalPeriod, 
        DateTime createdAt
    ) 
    {
        var pricingDetail = RentalPriceService.CalcTotalRentalPrice(car, rentalPeriod);

        var rental = new Rental(
            Guid.NewGuid(),
            car.Id, 
            userId,
            rentalPeriod,
            pricingDetail.BasePrice,
            pricingDetail.MaintenancePrice,
            pricingDetail.AccesoriesPrice,
            pricingDetail.TotalPrice,
            RentalStatus.Booked,
            createdAt
        );

        rental.RaiseDomainEvent(new BookedRentalDomainEvent(rental.Id));

        car.LastRentalDate = createdAt;

        return rental; 
    }

    public Result Confirm (DateTime utcNow) 
    {
        // To confirm, rental must get the state booked previously this op
        if(Status != RentalStatus.Booked)
        {
            return Result.Failure(RentalErrors.NotBooked); 
        }

        Status = RentalStatus.Confirmed; 
        ConfirmationDate = utcNow;

        RaiseDomainEvent(new ConfirmedRentalDomainEvent(Id)); 

        return Result.Success(); 
    }

    public Result RejectBooking (DateTime utcNow) 
    {
        if(Status != RentalStatus.Booked)
        {
            return Result.Failure(RentalErrors.NotBooked); 
        }

        Status = RentalStatus.Rejected; 
        BookingRejectDate = utcNow;

        RaiseDomainEvent(new RejectedBookingRentalDomainEvent(Id)); 

        return Result.Success();
    }

    public Result Cancel (DateTime utcNow) 
    {
        if(Status != RentalStatus.Confirmed)
        {
            return Result.Failure(RentalErrors.NotConfirmed); 
        }

        var currentDate = DateOnly.FromDateTime(utcNow);

        if(currentDate > RentalPeriod.Start) 
        {
            return Result.Failure(RentalErrors.OnGoing); 
        }

        Status = RentalStatus.Cancelled; 
        CancellationDate = utcNow;

        RaiseDomainEvent(new CancelRentalDomainEvent(Id)); 

        return Result.Success();
    }

    public Result Complete (DateTime utcNow) 
    {
        if(Status != RentalStatus.Confirmed)
        {
            return Result.Failure(RentalErrors.NotConfirmed); 
        }

        Status = RentalStatus.Completed; 
        CompletationDate = utcNow;

        RaiseDomainEvent(new CompletedRentalDomainEvent(Id)); 

        return Result.Success();
    }

}