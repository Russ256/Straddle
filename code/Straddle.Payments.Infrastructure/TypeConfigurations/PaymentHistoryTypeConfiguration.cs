namespace Straddle.Payments.Infrastructure.TypeConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Straddle.Payments.Domain.Model;

internal class PaymentHistoryTypeConfiguration : IEntityTypeConfiguration<PaymentHistory>
{
    public void Configure(EntityTypeBuilder<PaymentHistory> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasConversion<PaymentHistoryIdValueConverter>();
        builder.Property(p => p.PaymentId).HasConversion<PaymentIdValueConverter>();
    }
}