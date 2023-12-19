namespace Straddle.Payments.Application.Commands;

using Straddle.Application.Commands;
using Straddle.Payments.Application.Dtos;

public record CreatePaymentRequest(decimal Amount, string FromAccount, string ToAccount, string Reference, DateOnly? Date = null) : ICommandRequest<CreatePaymentResponse>;

public record CreatePaymentResponse(PaymentDto Payment);