using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Rentals.Errors;

public static class RentalErrors 
{
    public static Error NotFound = new Error("Rental.NotFound", "The Rental with provided id was not found.");

    public static Error Overlap = new Error("Rental.Overlap", "The resource its not available for rental at the requested date."); 

    public static Error NotBooked = new Error("Rental.NotBooked", "Booking its required to proceed.");

    public static Error NotConfirmed = new Error("Rental.NotConfirmed", "Confirmation its required to proceed");

    public static Error OnGoing = new Error("Rental.OnGoing", "The rental has already starded.");
}