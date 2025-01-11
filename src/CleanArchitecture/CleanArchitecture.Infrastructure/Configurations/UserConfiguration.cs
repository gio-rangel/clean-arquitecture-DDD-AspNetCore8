using CleanArchitecture.Domain.Users.Entities;
using CleanArchitecture.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");

        builder.HasKey((user) => user.Id);

        builder.Property(user => user.FirstName)
            .HasMaxLength(200)
            .HasConversion(firstName => 
                firstName!.Value, value => new FirstName(value)
            ) // convierte el object value en un valor primitivo accediendo y mapeando sus propiedad (unica)
        ;

        builder.Property(user => user.LastName)
            .HasMaxLength(200)
            .HasConversion(lastName => 
                lastName!.Value, value => new LastName(value)
            ) // convierte el object value en un valor primitivo accediendo y mapeando sus propiedad (unica)
        ;

        builder.Property(user => user.Email)
            .HasMaxLength(400)
            .HasConversion(email => 
                email!.Value, value => new Domain.Users.ValueObjects.Email(value)
            ) // convierte el object value en un valor primitivo accediendo y mapeando sus propiedad (unica)
        ;

        builder.HasIndex(user => user.Email).IsUnique();
    }
}