using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Reviews.Events;
using CleanArchitecture.Domain.Rentals.Entities;
using CleanArchitecture.Domain.Rentals.Errors;
using CleanArchitecture.Domain.Reviews.ValueObjects;
using CleanArchitecture.Domain.Rentals.Enums;

namespace CleanArchitecture.Domain.Reviews.Entities;

public sealed class Review : Entity 
{
    private Review(
        Guid id, 
        Guid rentalId, 
        Guid userId, 
        Guid carId,
        Rating rating,
        Commentary commentary,
        DateTime createdAt
    ) : base(id) 
    {
        Id = id;
        CarId = carId;
        RentalId = rentalId;
        UserId = userId;
        Rating = rating;
        Commentary = commentary;
        CreatedAt = createdAt;
    }

    public Guid CarId { get; private set; }
    public Guid RentalId { get; private set; }
    public Guid UserId { get; private set; }
    public Rating? Rating { get; private set; }
    public Commentary? Commentary { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static Result<Review> Create(Rental rental, Rating ratingValue, Commentary commentary, DateTime nowUtc)
    {
        if(rental.Status != RentalStatus.Completed)
        {
            return Result.Failure<Review>(ReviewErrors.NotEligible); 
        }

        var review = new Review(
            Guid.NewGuid(),
            rental.Id,
            rental.UserId,
            rental.CarId,
            ratingValue,
            commentary,
            nowUtc
        );

        review.RaiseDomainEvent(new ReviewCreatedDomainEvent(review.Id)); 

        return Result.Success(review);
    }
}