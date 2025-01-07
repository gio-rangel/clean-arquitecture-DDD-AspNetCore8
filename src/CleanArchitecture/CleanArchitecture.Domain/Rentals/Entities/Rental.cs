using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Cars.Entities;

namespace CleanArchitecture.Domain.Rentals.Entities;

public sealed class Rental : Entity 
{ 
    private Rental (
        Guid id,
        Guid carId,
        Guid userId,
        DateRange rentalPeriod,
        Currency basePrice,
        Currency manteinancePrice,
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
        ManteinancePrice = manteinancePrice;
        AccesoriesPrice = accesoriesPrice;
        FinalPrice = finalPrice;
        Status = status;
        CreatedAt = createdAt;
    }

    public Guid CarId { get; private set; }
    public Guid UserId { get; private set; }
    // BasePrice es el precio base (dias*precio de renta del auto)
    public Currency BasePrice { get; private set; }
    public Currency ManteinancePrice { get; private set; }
    public Currency AccesoriesPrice { get; private set; }
    public Currency FinalPrice { get; private set; }
    public RentalStatus Status { get; private set; }
    public DateRange RentalPeriod { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ConfirmationDate { get; private set; }
    public DateTime BookingCancellationDate { get; private set; }
    public DateTime CompletationDate { get; private set; }
    public DateTime CancellationDate { get; private set; }

    public static Rental Book (
        Guid carId,
        Guid userId,
        DateRange rentalPeriod,
        PricingDetail pricingDetail,
        DateTime createdAt
    ) 
    {
        // Esta logica (asignacion de datos) va en la capa de aplicacion... 
        // get car by id; 
        // var car; 
        // var pricingDetail = RentalPriceService.CalcTotalRentalPrice(car, rentalPeriod);

        return new Rental(
            Guid.NewGuid(),
            carId, 
            userId,
            rentalPeriod,
            pricingDetail.BasePrice,
            pricingDetail.ManteinancePrice,
            pricingDetail.AccesoriesPrice,
            pricingDetail.TotalPrice,
            RentalStatus.Booked,
            createdAt
        );
    }

    public static void ConfirmBooking () {}

    public static void CancellBooking () {}

    public static void Cancell () {}

    //  public static void Completation () {}
}