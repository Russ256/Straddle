namespace Straddle.Application.Commands;

using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Base class for all commands
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public abstract class Command<TRequest, TResponse> : IRequestHandler<TRequest, CommandResponse<TResponse>>
    where TRequest : IRequest<CommandResponse<TResponse>>
{
    private readonly ILogger? _logger;

    public Command()
    {
    }

    public Command(ILogger logger)
    {
        _logger = logger;
    }

    public ILogger Logger
    {
        get
        {
            return _logger ?? throw new LoggerNotSetException();
        }
    }

    public abstract Task<CommandResponse<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Command failure
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="errorCode"></param>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    protected CommandResponse<TResponse> Error(string propertyName, string errorCode, string errorMessage)
    {
        CommandResponse<TResponse> response = new CommandResponse<TResponse>();
        response.Errors.Add(new ValidationFailure(propertyName, errorMessage) { ErrorCode = errorCode });
        return response;
    }

    /// <summary>
    /// Command failure with multiple validation errors.
    /// </summary>
    /// <param name="validationFailures"></param>
    /// <returns></returns>
    protected CommandResponse<TResponse> Error(IEnumerable<ValidationFailure> validationFailures)
    {
        CommandResponse<TResponse> response = new CommandResponse<TResponse>();
        foreach (ValidationFailure validationFailure in validationFailures)
        {
            response.Errors.Add(validationFailure);
        }
        return response;
    }

    /// <summary>
    /// Command failure
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="errorCode"></param>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    protected CommandResponse<TResponse> Error(string errorCode, string errorMessage)
    {
        CommandResponse<TResponse> response = new CommandResponse<TResponse>();
        response.Errors.Add(new ValidationFailure(string.Empty, errorMessage) { ErrorCode = errorCode });
        return response;
    }

    /// <summary>
    /// Return a not found error
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="errorCode"></param>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    protected CommandResponse<TResponse> NotFound(string name)
    {
        CommandResponse<TResponse> response = new CommandResponse<TResponse>();
        response.Errors.Add(new ValidationFailure(name, $"{name} not found.") { ErrorCode = "ERR0001" });
        return response;
    }

    /// <summary>
    /// Command completed successfully
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    protected CommandResponse<TResponse> Ok(TResponse data)
    {
        return new CommandResponse<TResponse>(data);
    }

    /// <summary>
    /// Command completed successfully no data
    /// </summary>
    /// <returns></returns>
    protected CommandResponse<TResponse> Ok()
    {
        return new CommandResponse<TResponse>();
    }

    /// <summary>
    /// Command completed successfully
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    protected Task<CommandResponse<TResponse>> OkFromResult(TResponse data)
    {
        return Task.FromResult(new CommandResponse<TResponse>(data));
    }

    /// <summary>
    /// Command completed successfully
    /// </summary>
    /// <returns></returns>
    protected Task<CommandResponse<TResponse>> OkFromResult()
    {
        return Task.FromResult(new CommandResponse<TResponse>());
    }
}