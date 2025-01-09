using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Rentals.Errors;

public static class CarErrors 
{
    public static Error NotFound = new Error("Car.Error", "Car with requested id wasnt found.");
}