namespace CleanArchitecture.Application.Rentals.GetRental;

/**
* Para enviar datos al cliente usamos primitivos. 
* Solo para modelar los datos usamos records u object values
*/
public sealed class RentalResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid CarId { get; init; }
    public int Status { get; init; }
    public decimal RentalPrice { get; init; }
    public string? RentalCurrencyType { get; init; }
    public decimal MaintenancePrice { get; init; }
    public string? MaintenanceCurrencyType { get; init; }
    public decimal AccesoriesPrice { get; init; }
    public string? AccesorriesCurrencyType { get; init; }
    public decimal FinalPrice { get; init; }
    public string? FinalCurrencyType { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public DateTime CreatedAt { get; init; }
}