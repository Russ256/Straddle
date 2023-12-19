namespace Straddle.Payments.Application.Commands;

using FluentValidation;
using Straddle.Payments.Domain.Data;

public class GetPaymentCommandValidator : AbstractValidator<GetPaymentRequest>
{
    private readonly IPaymentReadRepository _paymentRepository;

    public GetPaymentCommandValidator(IPaymentReadRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;

        RuleFor(p => p.Id)
            .MustAsync(async (id, cancellationToken) => await _paymentRepository.AnyAsync(p => p.Id == id, cancellationToken))
            .WithMessage("Payment does not exist.")
            .WithErrorCode("404");
    }
}