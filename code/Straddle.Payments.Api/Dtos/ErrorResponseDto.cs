namespace Straddle.Payments.Api.Dtos;

/// <summary>
/// Error detail
/// </summary>
public class ErrorDto
{
    /// <summary>
    /// Construst a new instance of an error
    /// </summary>
    /// <param name="code">The errors code</param>
    /// <param name="message">The errors description</param>
    public ErrorDto(string code, string message)
    {
        Code = code;
        Message = message;
    }

    /// <summary>
    /// The error code
    /// </summary>
    public string Code { get; private set; }

    /// <summary>
    /// The description of the error
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// Returns the string to represent the current object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{Code}: {Message}";
    }
}

/// <summary>
/// Request errors
/// </summary>
public class ErrorResponseDto
{
    /// <summary>
    /// The list of errors found in the request
    /// </summary>
    public ErrorDto[]? Errors { get; set; }
}