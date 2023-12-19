namespace Straddle.Payments.Domain.Data;

using Straddle.Payments.Domain.Model;

public interface IPaymentHistoryWriter
{
    public void Write(PaymentId paymentId, PaymentHistoryType type);
}