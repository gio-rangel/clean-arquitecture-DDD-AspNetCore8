using CleanArchitecture.Application.Abstractions.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Abstractions.Behaviors;

// Handling cross-cutting concerns
public class LoggingBehavior<TRequest, TResponse>
: IPipelineBehavior<TRequest, TResponse>
where TRequest : IBaseCommand
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken
    )
    {
        var name = request.GetType().Name;

        try
        {
            _logger.LogInformation($"Executing command request {name}.");
            var result = await next();
            _logger.LogInformation($"The command {name} was executed succesfully.");

            return result;
        } catch (Exception exception) 
        {
            _logger.LogError(exception, $"The command {name} was executed with errors.");
            throw; 
        }
    }
}