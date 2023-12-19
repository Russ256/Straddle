namespace Straddle.Payments.Infrastructure.Data;

using DataRepositoryCore;
using Straddle.Payments.Domain.Data;
using Straddle.Payments.Domain.Model;

public class PaymentReadRepository : ReadDataRepository<Payment, PaymentId>, IPaymentReadRepository
{
    public PaymentReadRepository(IDataContext dataContext)
        : base(dataContext)
    {
    }
}