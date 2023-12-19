namespace Straddle.Payments.Domain.Data;

using DataRepositoryCore;
using Straddle.Payments.Domain.Model;

public interface IPaymentHistoryUpdateRepository : IDataRepository<PaymentHistory, PaymentHistoryId>;