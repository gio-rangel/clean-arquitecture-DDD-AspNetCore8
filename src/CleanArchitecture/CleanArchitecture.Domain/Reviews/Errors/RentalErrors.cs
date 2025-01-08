using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Rentals.Errors;

public static class ReviewErrors 
{
    public static Error NotEligible = new Error("Review.NotEligible", "The rental has to be completed to leave a review.");
}