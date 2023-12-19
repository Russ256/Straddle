namespace Straddle.Payments.Application.Commands;

using Microsoft.Extensions.Logging;
using Straddle.Application.Commands;
using Straddle.Payments.Domain.Data;
using Straddle.Payments.Domain.Model;
using System.Threading.Tasks;

public class ProcessPaymentCheckCommand : Command<ProcessPaymentCheckRequest, ProcessPaymentCheckResponse>
{
    private readonly IPaymentUpdateRepository _paymentRepository;
    private readonly IPaymentHistoryWriter _paymentHistoryWriter;

    public ProcessPaymentCheckCommand(ILogger<ProcessPaymentCommand> logger, IPaymentUpdateRepository paymentRepository, IPaymentHistoryWriter paymentHistoryWriter)
        : base(logger)
    {
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
        _paymentHistoryWriter = paymentHistoryWriter ?? throw new ArgumentNullException(nameof(paymentHistoryWriter));
    }

    public override async Task<CommandResponse<ProcessPaymentCheckResponse>> Handle(ProcessPaymentCheckRequest request, CancellationToken cancellationToken)
    {
        Logger.LogTrace("Process payment check {paymentId}", request.Id);

        Payment payment = await _paymentRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (payment.Status != PaymentStatus.Processing)
        {
            return Ok();
        }
        // This is where we would call the bank to check the payment status.

        payment.Status = PaymentStatus.Completed;

        _paymentHistoryWriter.Write(payment.Id, PaymentHistoryType.Completed);

        Logger.LogTrace("Payment {paymentId} complete", request.Id);

        return Ok();
    }
}