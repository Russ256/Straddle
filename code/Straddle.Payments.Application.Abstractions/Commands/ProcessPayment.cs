namespace Straddle.Payments.Application.Commands;

using Straddle.Application.Commands;

public record ProcessPaymentRequest(Guid Id) : ICommandRequest<ProcessPaymentResponse>;

public record ProcessPaymentResponse();