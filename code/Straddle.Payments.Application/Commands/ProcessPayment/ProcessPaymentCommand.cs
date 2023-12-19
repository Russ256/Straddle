namespace Straddle.Payments.Application.Commands;

using Microsoft.Extensions.Logging;
using Straddle.Application.Commands;
using Straddle.Payments.Domain.Data;
using Straddle.Payments.Domain.Messages;
using Straddle.Payments.Domain.Model;
using Straddle.Payments.Domain.Services;
using System.Threading.Tasks;

public class ProcessPaymentCommand : Command<ProcessPaymentRequest, ProcessPaymentResponse>
{
    private readonly IPaymentUpdateRepository _paymentRepository;
    private readonly IPaymentHistoryWriter _paymentHistoryWriter;
    private readonly IPublisher _publisher;

    public ProcessPaymentCommand(ILogger<ProcessPaymentCommand> logger,
                                 IPaymentUpdateRepository paymentRepository,
                                 IPaymentHistoryWriter paymentHistoryWriter,
                                 IPublisher publisher)
        : base(logger)
    {
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
        _paymentHistoryWriter = paymentHistoryWriter ?? throw new ArgumentNullException(nameof(paymentHistoryWriter));
        _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
    }

    public override async Task<CommandResponse<ProcessPaymentResponse>> Handle(ProcessPaymentRequest request, CancellationToken cancellationToken)
    {
        Logger.LogTrace("Processing payment {paymentId}", request.Id);

        Payment payment = await _paymentRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (payment.Status != PaymentStatus.Pending)
        {
            return Ok();
        }

        // This is where we would call the bank to process the payment.

        payment.Status = PaymentStatus.Processing;

        _paymentHistoryWriter.Write(payment.Id, PaymentHistoryType.Processing);

        _publisher.Publish(request.Id, new PaymentProcessing(request.Id), DateTimeOffset.Now.AddMinutes(1));

        return Ok();
    }
}