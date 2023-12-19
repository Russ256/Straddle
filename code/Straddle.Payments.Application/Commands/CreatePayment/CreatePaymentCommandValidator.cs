namespace Straddle.Payments.Application.Commands;

using FluentValidation;
using Straddle.Payments.Domain.Data;

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentRequest>
{
    public CreatePaymentCommandValidator(IPaymentReadRepository paymentReadRepository)
    {
        ArgumentNullException.ThrowIfNull(nameof(paymentReadRepository));

        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.FromAccount).NotEmpty();
        RuleFor(x => x.ToAccount).NotEmpty();
        RuleFor(x => x.Reference).NotEmpty()
                                 .MustAsync(async (reference, cancellation) =>
                                 {
                                     return !await paymentReadRepository.AnyAsync(p => p.Reference == reference, cancellation);
                                 }).WithMessage("Duplicate reference")
                                 .WithErrorCode("409");
        RuleFor(x => x.Date).NotEmpty()
                            .Must(x => x >= DateOnly.FromDateTime(DateTime.UtcNow))
                                        .WithMessage("Date must be in the future")
                                        .WithErrorCode("400");
    }
}