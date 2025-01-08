using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Reviews.ValueObjects;

public sealed record Rating
{
    // construct written in functianal notation
    private Rating (int value) => Value = value;

    public int Value { get; private set;}
    public static readonly Error Invalid = new("Rating.Invalid", "Rating must be a positive value between 1 and 5.");

    public static Result<Rating> Create(int value)
    {
        if (value < 0 && value > 6)
        {
            return Result.Failure<Rating>(Invalid); 
        }

        return new Rating (value);
    }
}