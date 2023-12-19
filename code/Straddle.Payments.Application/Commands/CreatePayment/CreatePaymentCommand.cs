namespace Straddle.Payments.Application.Commands;

using Microsoft.Extensions.Logging;
using Straddle.Application.Commands;
using Straddle.Payments.Application.Dtos;
using Straddle.Payments.Domain.Data;
using Straddle.Payments.Domain.Messages;
using Straddle.Payments.Domain.Model;
using Straddle.Payments.Domain.Services;
using System;
using System.Threading.Tasks;

public class CreatePaymentCommand : Command<CreatePaymentRequest, CreatePaymentResponse>
{
    private readonly IPaymentUpdateRepository _paymentRepository;
    private readonly IPublisher _publisher;
    private readonly IPaymentHistoryWriter _paymentHistoryWriter;

    public CreatePaymentCommand(ILogger<CreatePaymentCommand> logger,
                                IPaymentUpdateRepository paymentRepository,
                                IPublisher publisher,
                                IPaymentHistoryWriter paymentHistoryWriter)
        : base(logger)
    {
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
        _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        _paymentHistoryWriter = paymentHistoryWriter ?? throw new ArgumentNullException(nameof(paymentHistoryWriter));
    }

    public override Task<CommandResponse<CreatePaymentResponse>> Handle(CreatePaymentRequest request, CancellationToken cancellationToken)
    {
        Logger.LogTrace("Creating payment reference {reference}", request.Reference);

        Payment payment = new()
        {
            Id = PaymentId.New(),
            Status = PaymentStatus.Pending,
            Amount = request.Amount,
            FromAccount = request.FromAccount,
            ToAccount = request.ToAccount,
            Reference = request.Reference,
            Date = request.Date ?? DateOnly.FromDateTime(DateTime.Now)
        };

        _paymentRepository.Add(payment);
        _publisher.Publish(payment.Id, new PaymentRequest(payment.Id), new DateTimeOffset(payment.Date, TimeOnly.MinValue, new TimeSpan()));
        _paymentHistoryWriter.Write(payment.Id, PaymentHistoryType.Created);

        Logger.LogTrace("Payment reference {reference} created", request.Reference);

        return OkFromResult(new CreatePaymentResponse(new PaymentDto(payment.Id,
                                                                     payment.Status.GetDescription(),
                                                                     payment.Amount,
                                                                     payment.FromAccount,
                                                                     payment.ToAccount,
                                                                     payment.Reference,
                                                                     payment.Date)));
    }
}