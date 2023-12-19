namespace Straddle.Payments.Infrastructure.Data;

using DataRepositoryCore;
using Straddle.Payments.Domain.Data;
using Straddle.Payments.Domain.Model;

public class PaymentHistoryUpdateRepository : DataRepository<PaymentHistory, PaymentHistoryId>, IPaymentHistoryUpdateRepository
{
    public PaymentHistoryUpdateRepository(IDataContext dataContext)
        : base(dataContext)
    {
    }
}