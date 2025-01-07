using CleanArchitecture.Domain.Cars.Entities;

namespace CleanArchitecture.Domain.Rentals.Entities;

public record PricingDetail (
    Currency BasePrice,
    Currency AccesoriesPrice,
    Currency ManteinancePrice,
    Currency TotalPrice
);