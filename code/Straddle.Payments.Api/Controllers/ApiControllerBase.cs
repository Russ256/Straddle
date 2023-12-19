namespace Straddle.Payments.Api.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Straddle.Application.Commands;
using Straddle.Payments.Api.Dtos;
using System.Net;
using System.Net.Mime;
using System.Threading;

[Authorize]
[ApiController]
[Route("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ApiControllerBase : ControllerBase
{
    public ApiControllerBase(IMediator mediator)
    {
        Mediator = mediator;
    }

    protected IMediator Mediator { get; }

    protected async Task<IActionResult> ExecuteCommandAsync<TResponse>(ICommandRequest<TResponse> request,
                                                                         CancellationToken cancellationToken,
                                                                         HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        CommandResponse<TResponse> response = await SendAsync(request, cancellationToken);
        if (response.HasErrors)
        {
            return HandleErrors(response);
        }

        return new StatusCodeResult((int)statusCode);
    }

    protected async Task<IActionResult> ExecuteCommandAsync<TResponse>(ICommandRequest<TResponse> request,
                                                                        Func<TResponse, object> success,
                                                                        CancellationToken cancellationToken,
                                                                        HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        CommandResponse<TResponse> response = await SendAsync(request, cancellationToken);
        if (response.HasErrors)
        {
            return HandleErrors(response);
        }

        if (response.Data is not null)
        {
            object successResponse = success(response.Data);
            return new JsonResult(successResponse) { StatusCode = (int)statusCode };
        }

        return new StatusCodeResult((int)statusCode);
    }

    private static IActionResult HandleErrors<TResponse>(CommandResponse<TResponse> response)
    {
        ErrorResponseDto errorResponse = new()
        {
            Errors = response.Errors.Select(error => new ErrorDto(error.ErrorCode, error.ErrorMessage)).ToArray()
        };

        if (response.Errors[0].ErrorCode == "404")
        {
            return new JsonResult(errorResponse) { StatusCode = StatusCodes.Status404NotFound };
        }
        else if (response.Errors[0].ErrorCode == "409")
        {
            return new JsonResult(errorResponse) { StatusCode = StatusCodes.Status409Conflict };
        }

        return new JsonResult(errorResponse) { StatusCode = StatusCodes.Status400BadRequest };
    }

    private async Task<CommandResponse<TResponse>> SendAsync<TResponse>(ICommandRequest<TResponse> request, CancellationToken cancellationToken)
    {
        CommandResponse<TResponse> response;

        try
        {
            response = (await Mediator.Send(request, cancellationToken)) ?? throw new Exception("Null returned from mediatr.");
        }
        catch (DbUpdateConcurrencyException)
        {
            response = new();
            response.AddError("Id", "409", "Failed to update as data has changed");
        }

        return response;
    }
}