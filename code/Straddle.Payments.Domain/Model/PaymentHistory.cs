namespace Straddle.Payments.Domain.Model;

using DataRepositoryCore;
using System;

public class PaymentHistory : IEntity<PaymentHistoryId>, ICreatedAt
{
    public PaymentHistoryId Id { get; set; }
    public PaymentId PaymentId { get; set; }
    public Payment Payment { get; set; }
    public string User { get; set; }
    public string Error { get; set; }
    public PaymentHistoryType Type { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public enum PaymentHistoryType : byte
{
    Created = 0,
    Processing = 1,
    Completed = 2,
    Cancelled = 3,
    CancelFailed = 4,
    Errored = 5,
}