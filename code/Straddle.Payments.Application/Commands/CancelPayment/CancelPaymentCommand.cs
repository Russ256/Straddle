namespace Straddle.Payments.Application.Commands;

using Microsoft.Extensions.Logging;
using Straddle.Application.Commands;
using Straddle.Payments.Domain.Data;
using Straddle.Payments.Domain.Model;
using System.Threading.Tasks;

public class CancelPaymentCommand : Command<CancelPaymentRequest, CancelPaymentResponse>
{
    private readonly IPaymentUpdateRepository _paymentRepository;
    private readonly IPaymentHistoryWriter _paymentHistoryWriter;

    public CancelPaymentCommand(ILogger<CancelPaymentCommand> logger, IPaymentUpdateRepository paymentRepository, IPaymentHistoryWriter paymentHistoryWriter)
        : base(logger)
    {
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
        _paymentHistoryWriter = paymentHistoryWriter ?? throw new ArgumentNullException(nameof(paymentHistoryWriter));
    }

    public override async Task<CommandResponse<CancelPaymentResponse>> Handle(CancelPaymentRequest request, CancellationToken cancellationToken)
    {
        Logger.LogTrace("Attempting to cancel payment {paymentId}", request.Id);

        Payment payment = await _paymentRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (payment is null)
        {
            return Error("404", "Payment not found");
        }

        if (payment.Status != PaymentStatus.Pending)
        {
            Logger.LogTrace("Unable to cancel payment {paymentId} with status {status}", request.Id, payment.Status);
            _paymentHistoryWriter.Write(payment.Id, PaymentHistoryType.CancelFailed);
            return Ok();
        }

        payment.Status = PaymentStatus.Cancelled;

        _paymentHistoryWriter.Write(payment.Id, PaymentHistoryType.Cancelled);

        Logger.LogTrace("Payment {paymentId} cancelled", request.Id);

        return Ok(new CancelPaymentResponse());
    }
}