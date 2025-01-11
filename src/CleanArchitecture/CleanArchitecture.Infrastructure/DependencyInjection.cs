using CleanArchitecture.Application.Abstractions.Clock;
using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Application.Abstractions.Emails;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Cars.Interfaces;
using CleanArchitecture.Domain.Rentals.Interfaces;
using CleanArchitecture.Domain.Users.Interfaces;
using CleanArchitecture.Infrastructure.Clock;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Email;
using CleanArchitecture.Infrastructure.Repositories;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure;

public static class DependendencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        /**
        * 'AddTransitent' es un método usado en el sistema de inyección de dependencias
        * registra un servicio con un tiempo de vida transitorio (transient). 
        * Esto significa que una nueva instancia del servicio será creada cada vez que se resuelva.
        */
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEmailService, EmailService>();

        var connectionString = configuration.GetConnectionString("Database")
            ?? throw new ArgumentNullException(nameof(configuration))
        ;

        services.AddDbContext<ApplicationDbContext>(options => {
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention(); // Al ejecutar la migracion hara que las convenciones de las tablas y columnas sean snake_case
        });

        /**
        * 'AddScoped' es un método usado en el sistema de inyección de dependencias
        * registra un servicio con un tiempo de vida de alcance (scoped). 
        * Esto significa que una única instancia del servicio será creada y compartida dentro del mismo alcance.
        */
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<IRentalRepository, RentalRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>()); 

        services.AddSingleton<ISqlConnectionFactory>( _ => new SqlConnectionfactory(connectionString));

        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        
        return services;
    }
}