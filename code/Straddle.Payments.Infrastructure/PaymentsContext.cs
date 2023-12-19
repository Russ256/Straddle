namespace Straddle.Payments.Infrastructure;

using DataRepositoryCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Straddle.Payments.Domain.Model;
using Straddle.Payments.Domain.Services;

public class PaymentsContext : DataContext
{
    private readonly IUserProvider _userProvider;

    public PaymentsContext(DbContextOptions options, IUserProvider userProvider)
        : base(options)
    {
        _userProvider = userProvider;
    }

    public DbSet<PaymentHistory> PaymentHistory { get; set; }
    public DbSet<Payment> Payments { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;

        foreach (IUpdatedAt item in ChangeTracker.Entries().Where(e => e.State == EntityState.Added).Select(s => s.Entity).OfType<IUpdatedAt>())
        {
            item.UpdatedAt = now;
        }

        foreach (EntityEntry item in ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).Where(item => item.Entity is IUpdatedAt))
        {
            item.OriginalValues["UpdatedAt"] = (item.Entity as IUpdatedAt)!.UpdatedAt;
            (item.Entity as IUpdatedAt)!.UpdatedAt = now;
        }

        foreach (ICreatedAt item in ChangeTracker.Entries().Where(e => e.State == EntityState.Added).Select(s => s.Entity).OfType<ICreatedAt>())
        {
            item.CreatedAt = now;
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentsContext).Assembly);
    }
}