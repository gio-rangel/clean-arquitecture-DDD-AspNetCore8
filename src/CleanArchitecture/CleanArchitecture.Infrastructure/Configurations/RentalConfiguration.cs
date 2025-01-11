using CleanArchitecture.Domain.Cars.Entities;
using CleanArchitecture.Domain.Rentals.Entities;
using CleanArchitecture.Domain.Shared.ValueObjects;
using CleanArchitecture.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations;

internal sealed class RentalConfiguration : IEntityTypeConfiguration<Rental>
{
    public void Configure(EntityTypeBuilder<Rental> builder)
    {
        builder.ToTable("rentals");

        builder.HasKey((rental) => rental.Id);

        builder.OwnsOne(rental => rental.BasePrice, priceBuilder => 
            priceBuilder.Property(currency => currency.CurrencyType)
                .HasConversion(currencyType => currencyType!.Code, code => CurrencyType.FromCode(code!))
        );

        builder.OwnsOne(rental => rental.MaintenancePrice, priceBuilder => 
            priceBuilder.Property(currency => currency.CurrencyType)
                .HasConversion(currencyType => currencyType!.Code, code => CurrencyType.FromCode(code!))
        );

        builder.OwnsOne(rental => rental.AccesoriesPrice, priceBuilder => 
            priceBuilder.Property(currency => currency.CurrencyType)
                .HasConversion(currencyType => currencyType!.Code, code => CurrencyType.FromCode(code!))
        );

        builder.OwnsOne(rental => rental.FinalPrice, priceBuilder => 
            priceBuilder.Property(currency => currency.CurrencyType)
                .HasConversion(currencyType => currencyType!.Code, code => CurrencyType.FromCode(code!))
        );

        builder.OwnsOne(rental => rental.RentalPeriod);

        builder.HasOne<Car>()
            .WithMany()
            .HasForeignKey(rental => rental.CarId)
        ;

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(rental => rental.UserId)
        ;
    }
}