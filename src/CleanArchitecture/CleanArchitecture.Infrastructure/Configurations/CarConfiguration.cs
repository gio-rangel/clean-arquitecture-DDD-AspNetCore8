using CleanArchitecture.Domain.Cars.Entities;
using CleanArchitecture.Domain.Cars.ValueObjects;
using CleanArchitecture.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations;

internal sealed class CarConfiguration : IEntityTypeConfiguration<Car>
{
    // Configuraciones para la tabla cars
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable("cars");

        builder.HasKey((car) => car.Id);

        builder.OwnsOne(car => car.Address); // convierte el object value en valores primitivos accediendo y mapeando sus propiedades a traves del método 'OwnsOne'

        builder.Property(car => car.Model)
            .HasMaxLength(200)
            .HasConversion(model => model!.Value, value => new Model(value)) // convierte el object value en un valor primitivo accediendo y mapeando sus propiedad (unica)
        ;

        builder.Property(car => car.Vin)
            .HasMaxLength(500)
            .HasConversion(vin => vin!.Value, value => new Vin(value))
        ;

        builder.OwnsOne(car => car.Price, priceBuilder => 
            priceBuilder.Property(currency => currency.CurrencyType)
                .HasConversion(currencyType => currencyType.Code, code => CurrencyType.FromCode(code!))
        );

        builder.OwnsOne(car => car.MaintenancePrice, priceBuilder => 
            priceBuilder.Property(currency => currency.CurrencyType)
                .HasConversion(currencyType => currencyType.Code, code => CurrencyType.FromCode(code!))
        );

        builder.Property<uint>("Version").IsRowVersion();
    }
}