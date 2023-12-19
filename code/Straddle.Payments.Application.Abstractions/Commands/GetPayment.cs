namespace Straddle.Payments.Application.Commands;

using Straddle.Application.Commands;
using Straddle.Payments.Application.Dtos;

public record GetPaymentRequest(Guid Id) : ICommandRequest<GetPaymentResponse>;

public record GetPaymentResponse(PaymentDto Payment);