namespace CleanArchitecture.Application.Cars.SearchCars.Responses;

public sealed class AddressResponse
{
    public string? Country { get; init; }
    public string? Province { get; init; }
    public string? City { get; init; }
    public string? Depto { get; init; }
    public string? Street { get; init; }
    public int? StreetNumber { get; init; }
}