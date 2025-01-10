using CleanArchitecture.Application.Abstractions.Behaviors;
using CleanArchitecture.Domain.Rentals.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddAplication (this IServiceCollection services)
    {
        /** 
        * Using Mediator Pattern for handle dependency injection configs 
        * is a behavioral design pattern that defines an object (the mediator) to encapsulate how a set of objects interact with each other
        * This reduces dependencies between objects and promotes loose coupling
        */

        services.AddMediatR(configuration => {
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        }); 

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly); 

        services.AddTransient<RentalPriceService>();

        return services; 
    }
}