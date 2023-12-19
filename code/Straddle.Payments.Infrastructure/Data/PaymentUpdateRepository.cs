namespace Straddle.Payments.Infrastructure.Data;

using DataRepositoryCore;
using Straddle.Payments.Domain.Data;
using Straddle.Payments.Domain.Model;

public class PaymentUpdateRepository : DataRepository<Payment, PaymentId>, IPaymentUpdateRepository
{
    public PaymentUpdateRepository(IDataContext dataContext)
        : base(dataContext)
    {
    }
}