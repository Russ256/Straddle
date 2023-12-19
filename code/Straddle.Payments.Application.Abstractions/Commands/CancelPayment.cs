namespace Straddle.Payments.Application.Commands;

using Straddle.Application.Commands;

public record CancelPaymentRequest(Guid Id) : ICommandRequest<CancelPaymentResponse>;

public record CancelPaymentResponse();