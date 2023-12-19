namespace Straddle.Payments.Application.Commands;

using FluentValidation;
using Straddle.Payments.Domain.Data;
using Straddle.Payments.Domain.Model;
using System.Net.Http.Headers;

public class CancelPaymentCommandValidator : AbstractValidator<CancelPaymentRequest>
{
    private readonly IPaymentReadRepository _paymentRepository;

    public CancelPaymentCommandValidator(IPaymentReadRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
        Payment? payment = null;

        RuleFor(p => p.Id)
            .MustAsync(async (id, cancellationToken) =>
            {
                payment = await _paymentRepository.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
                return payment is not null;
            })
            .WithMessage("Payment does not exist")
            .WithErrorCode("404")
            .Must(id => payment is not null && payment.Status == PaymentStatus.Pending )
            .WithMessage("Can only cancel pending payments")
            .WithErrorCode("400");

    }
}