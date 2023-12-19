namespace Straddle.Payments.Domain.Model;

using System;

public readonly record struct PaymentId : IEquatable<PaymentId>
{
    public Guid Value { get; }

    public PaymentId(Guid value)
    {
        Value = value;
    }

    public static PaymentId New()
    {
        return new PaymentId(Guid.NewGuid());
    }

    public static implicit operator Guid(PaymentId value) => value.Value;
    public static explicit operator PaymentId(Guid value) => new(value);
}