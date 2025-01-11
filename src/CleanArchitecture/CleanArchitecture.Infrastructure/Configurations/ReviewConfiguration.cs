using CleanArchitecture.Domain.Cars.Entities;
using CleanArchitecture.Domain.Rentals.Entities;
using CleanArchitecture.Domain.Reviews.Entities;
using CleanArchitecture.Domain.Reviews.ValueObjects;
using CleanArchitecture.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations;

internal sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("reviews");

        builder.HasKey((review) => review.Id);

        builder.Property(review => review.Rating)
            .HasConversion(rating => 
                rating!.Value, value => Rating.Create(value).Value
            )
        ;

        builder.Property(review => review.Commentary)
            .HasMaxLength(255)
            .HasConversion(commentary => 
                commentary!.Value, value => new Commentary(value)
            )
        ;

        builder.HasOne<Rental>()
            .WithMany()
            .HasForeignKey(review => review.RentalId)
        ;

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(review => review.UserId)
        ;

        builder.HasOne<Car>()
            .WithMany()
            .HasForeignKey(review => review.CarId)
        ;
    }
}