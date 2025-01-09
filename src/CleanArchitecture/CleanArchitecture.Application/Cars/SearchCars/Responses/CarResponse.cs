namespace CleanArchitecture.Application.Cars.SearchCars.Responses;

public sealed class CarResponse 
{
    public Guid Id { get; init; }
    public string? Model { get; init; }
    public string? Vin { get; init; }
    public decimal Price { get; init; }
    public string? PriceCurrency { get; init; }
    public AddressResponse? Address { get; set; }
}