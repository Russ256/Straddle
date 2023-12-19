namespace Straddle.Payments.Application.Commands;

using Straddle.Application.Commands;
using Straddle.Payments.Application.Dtos;
using Straddle.Payments.Domain.Data;
using Straddle.Payments.Domain.Model;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class GetPaymentCommand : Command<GetPaymentRequest, GetPaymentResponse>
{
    private readonly IPaymentReadRepository _paymentRepository;

    public GetPaymentCommand(IPaymentReadRepository paymentRepository)
    {
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
    }

    public override async Task<CommandResponse<GetPaymentResponse>> Handle(GetPaymentRequest request, CancellationToken cancellationToken)
    {
        Payment? payment = await _paymentRepository.AsQueryable().Include(p => p.History).FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (payment is null)
        {
            return Error("404", "Payment not found");
        }

        IEnumerable<PaymentHistoryDto> history = payment.History.Select(p => new PaymentHistoryDto(p.Id, p.Type.GetDescription(), p.User, p.CreatedAt))
                                                                .OrderBy(p => p.CreatedAt);
        
        return Ok(new GetPaymentResponse(new PaymentDto(payment.Id,
                                                        payment.Status.GetDescription(),
                                                        payment.Amount,
                                                        payment.FromAccount,
                                                        payment.ToAccount,
                                                        payment.Reference,
                                                        payment.Date,
                                                        history)));
    }
}