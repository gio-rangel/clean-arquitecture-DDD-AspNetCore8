using CleanArchitecture.Domain.Cars.Entities;
using  CleanArchitecture.Domain.Cars.ValueObjects;
using CleanArchitecture.Domain.Rentals.ValueObjects;
using CleanArchitecture.Domain.Shared.ValueObjects;


namespace CleanArchitecture.Domain.Rentals.Services;


public static class RentalPriceService 
{
    public static PricingDetail CalcTotalRentalPrice(Car car, DateRange rentalPeriod) 
    {
        var basePrice = CalcBasePrice(car, rentalPeriod);
        // Extra charges
        var accesoryPrice = CalcAccesoriesTotalPrice(car, basePrice);
        var manteinancePrice = CalcManteinancePrice(car, basePrice);
        
        var totalPrice = basePrice + accesoryPrice + manteinancePrice;
        
        var pricingDetail = new PricingDetail(basePrice, accesoryPrice, manteinancePrice, totalPrice);

        return pricingDetail;
    }

    private static Currency CalcBasePrice (Car car, DateRange rentalPeriod)
    {
        if (car.Price == null)
        {
            throw new InvalidOperationException("Car price is required.");
        }

        var currencyType = car.Price!.CurrencyType;
        var period = rentalPeriod.Days * car.Price.Amount;

        return new Currency(period, currencyType);
    }

    /**
    * The static method 'CalcAccesoriesTotalPrice' accumulates the charge for every accesory added to the car rental
    * NOTES:
    * The value 0.5m represents a decimal literal in C#. 
    * The m suffix specifies that the number is a decimal type rather than a double or float.
    */
    private static Currency CalcAccesoriesTotalPrice (Car car, Currency rentalPeriodPrice)
    {
        CurrencyType currencyType = rentalPeriodPrice.CurrencyType; 
        decimal percentagePriceAcumulator = 0;

        foreach (var accesory in car.Accesories)
        {
            percentagePriceAcumulator += accesory switch
            {
                Accesory.AppleCar or Accesory.AndroidCar => 0.5m,
                Accesory.AirConditioning => 0.01m,
                Accesory.Maps => 0.01m,
                Accesory.Wifi => 0.03m,
                _ => 0
            };
        }

        var accesoryCharges = Currency.Zero(currencyType); 

        if(percentagePriceAcumulator > 0)
        {
            decimal accesoryPricePerPeriod = Math.Round(rentalPeriodPrice.Amount * percentagePriceAcumulator, 2);

            accesoryCharges = new Currency(accesoryPricePerPeriod, currencyType);
        }

        return  accesoryCharges; 
    }

    private static Currency CalcManteinancePrice (Car car, Currency rentalPeriodPrice) 
    {
        if (car.MaintenancePrice != null)
        {
            return car.MaintenancePrice;
        } 
        
        return Currency.Zero(rentalPeriodPrice.CurrencyType); 
    }
}