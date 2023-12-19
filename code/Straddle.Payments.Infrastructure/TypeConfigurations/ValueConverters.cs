namespace Straddle.Payments.Infrastructure.TypeConfigurations;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Straddle.Payments.Domain.Model;
using System;

internal class PaymentIdValueConverter : ValueConverter<PaymentId, Guid>
{
    public PaymentIdValueConverter() : base(v => v, v => new PaymentId(v))
    {
    }
}

internal class PaymentHistoryIdValueConverter : ValueConverter<PaymentHistoryId, Guid>
{
    public PaymentHistoryIdValueConverter() : base(v => v, v => new PaymentHistoryId(v))
    {
    }
}