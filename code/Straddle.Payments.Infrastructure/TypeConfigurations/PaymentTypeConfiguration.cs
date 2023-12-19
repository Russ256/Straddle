namespace Straddle.Payments.Infrastructure.TypeConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Straddle.Payments.Domain.Model;

internal class PaymentTypeConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasConversion<PaymentIdValueConverter>();
        builder.Property(p => p.Amount).HasPrecision(12, 2);
        builder.Property(p => p.FromAccount).HasMaxLength(20);
        builder.Property(p => p.ToAccount).HasMaxLength(20);
        builder.Property(p => p.Reference).HasMaxLength(20);
        builder.Property(p => p.UpdatedAt).IsConcurrencyToken();
    }
}
