namespace Straddle.Payments.Domain.Data;

using DataRepositoryCore;
using Straddle.Payments.Domain.Model;

public interface IPaymentUpdateRepository : IDataRepository<Payment, PaymentId>; 
