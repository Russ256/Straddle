namespace Straddle.Application.Commands;

using FluentValidation.Results;
using System.Collections.Generic;

public class CommandResponse
{
    private List<ValidationFailure>? _errors;

    public IList<ValidationFailure> Errors
    {
        get
        {
            _errors ??= new List<ValidationFailure>();

            return _errors;
        }
    }

    public bool HasErrors => _errors != null && _errors.Count != 0;

    public void AddError(string propertyName, string errorCode, string errorMessage)
    {
        Errors.Add(new ValidationFailure(propertyName, errorMessage) { ErrorCode = errorCode });
    }
}

public class CommandResponse<TData> : CommandResponse
{
    public CommandResponse()
    {
    }

    public CommandResponse(TData data)
    {
        Data = data;
    }

    public TData? Data { get; }
}