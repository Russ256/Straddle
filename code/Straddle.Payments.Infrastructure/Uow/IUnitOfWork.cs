namespace Straddle.Payments.Infrastructure.Uow;

using System.Threading.Tasks;

public interface IUnitOfWork
{
    Task BeginAsync(CancellationToken cancellationToken);

    Task CommitAsync(CancellationToken cancellationToken);

    Task RollbackAsync(CancellationToken cancellationToken);
}