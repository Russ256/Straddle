namespace Straddle.Payments.Domain.Model;

using DataRepositoryCore;

public class Payment : IEntity<PaymentId>, IUpdatedAt
{
    public decimal Amount { get; set; }
    public PaymentId Id { get; set; }
    public PaymentStatus Status { get; set; }
    public string FromAccount { get; set; }
    public string ToAccount { get; set; }
    public string Reference { get; set; }
    public DateOnly Date { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public ICollection<PaymentHistory> History { get; set; } = new HashSet<PaymentHistory>();
}

public enum PaymentStatus : byte
{
    Pending = 0,
    Processing = 1,
    Completed = 2,
    Cancelled = 3,
    Errored = 4,
}