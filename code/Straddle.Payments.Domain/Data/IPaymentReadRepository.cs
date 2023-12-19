namespace Straddle.Payments.Domain.Data;

using DataRepositoryCore;

using Straddle.Payments.Domain.Model;

public interface IPaymentReadRepository : IReadDataRepository<Payment, PaymentId>
{ }