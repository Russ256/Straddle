namespace Straddle.Payments.Infrastructure.Data;

using Straddle.Payments.Domain.Data;
using Straddle.Payments.Domain.Model;
using Straddle.Payments.Domain.Services;
using System;

internal class PaymentHistoryWriter : IPaymentHistoryWriter
{
    private readonly IPaymentHistoryUpdateRepository _paymentHistoryUpdateRepository;
    private readonly IUserProvider _userProvider;

    public PaymentHistoryWriter(IPaymentHistoryUpdateRepository paymentHistoryUpdateRepository, IUserProvider userProvider)
    {
        _paymentHistoryUpdateRepository = paymentHistoryUpdateRepository ?? throw new ArgumentNullException(nameof(paymentHistoryUpdateRepository));
        _userProvider = userProvider;
    }

    public void Write(PaymentId paymentId, PaymentHistoryType type)
    {
        PaymentHistory paymentHistory = new()
        {
            Id = PaymentHistoryId.New(),
            PaymentId = paymentId,
            Type = type,
            User = _userProvider.Name
        };

        _paymentHistoryUpdateRepository.Add(paymentHistory);
    }
}