using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Application.Abstractions.Messaging;
using FluentValidation;
using MediatR;

namespace CleanArchitecture.Application.Abstractions.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
: IPipelineBehavior<TRequest, TResponse>
where TRequest : IBaseCommand
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken
    )
    {
        if(!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        /**
        * Tomamos el contexto y seleccionamos todos aquellas validaciones que sean de tipo error
        * Luego mapeamos el objeto validation failure a un custom validate method que vamos a crear
        * Retornamos el resultado en forma de lista
        */
        var validationErrors = _validators
            .Select(validators => validators.Validate(context))
            .Where(validationResult => validationResult.Errors.Any())
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure => new ValidationError(
                validationFailure.PropertyName,
                validationFailure.ErrorMessage
            ))
            .ToList()
        ;

        if(validationErrors.Any())
        {
            throw new Exceptions.ValidationException(validationErrors);
        } 

        return await next();
    }
}