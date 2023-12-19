namespace Straddle.Application.Behaviors;

using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Straddle.Application.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Runs the registered validators for the request before continuing the pipeline
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : CommandResponse, new()
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators ?? throw new ArgumentNullException(nameof(validators));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Check for errors
        ValidationContext<TRequest> context = new(request);
        List<ValidationFailure>? failures = null;
        foreach (IValidator<TRequest> validator in _validators)
        {
            ValidationResult result = await validator.ValidateAsync(context, cancellationToken);
            if (!result.IsValid)
            {
                failures = result.Errors;
                break;
            }
        }

        if (failures is not null)
        {
            TResponse errorResponse = new();
            failures.ForEach(errorResponse.Errors.Add);
            return errorResponse;
        }

        TResponse response = await next();
        return response;
    }
}