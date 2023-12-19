namespace Straddle.Payments.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Straddle.Payments.Api.Converters;
using Straddle.Payments.Api.Dtos;
using Straddle.Payments.Application.Commands;
using System.Net;

[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class PaymentController : ApiControllerBase
{
    public PaymentController(IMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost(Name = "CreatePayment")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public Task<IActionResult> CreateAsync([FromBody] PaymentDto request, CancellationToken cancellationToken)
    {
        return ExecuteCommandAsync(new CreatePaymentRequest(request.Amount.GetValueOrDefault(),
                                                            request.FromAccount!,
                                                            request.ToAccount!,
                                                            request.Reference!,
                                                            request.Date),
                                   response =>
                                   {
                                       Response.Headers.Location = $"{Request.Scheme}://{Request.Host}{Request.Path.ToUriComponent()}/{response.Payment.Id}";
                                       return response.Payment.ToDto();
                                   },
                                   statusCode: HttpStatusCode.Accepted,
                                   cancellationToken: cancellationToken);
    }

    [HttpGet(template: "{id:guid}", Name = "GetPayment")]
    [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return ExecuteCommandAsync(new GetPaymentRequest(id),
                                   response => response.Payment.ToDto(),
                                   cancellationToken: cancellationToken);
    }

    [HttpPost(template: "{id:guid}/cancel", Name = "CancelPayment")]
    [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> CancelAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return ExecuteCommandAsync(new CancelPaymentRequest(id),
                                   cancellationToken: cancellationToken);
    }
}