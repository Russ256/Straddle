namespace Straddle.Payments.Domain.Model;

using System;

public readonly record struct PaymentHistoryId : IEquatable<PaymentHistoryId>
{
    public Guid Value { get; }

    public PaymentHistoryId(Guid value)
    {
        Value = value;
    }

    public static PaymentHistoryId New()
    {
        return new PaymentHistoryId(Guid.NewGuid());
    }

    public static implicit operator Guid(PaymentHistoryId value) => value.Value;
    public static explicit operator PaymentHistoryId(Guid value) => new(value);
}