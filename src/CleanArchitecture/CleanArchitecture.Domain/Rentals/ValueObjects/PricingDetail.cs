using CleanArchitecture.Domain.Shared.ValueObjects;

namespace CleanArchitecture.Domain.Rentals.ValueObjects;

public record PricingDetail (
    Currency BasePrice,
    Currency AccesoriesPrice,
    Currency ManteinancePrice,
    Currency TotalPrice
);