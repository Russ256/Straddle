namespace Straddle.Payments.Infrastructure.Uow;

using DataRepositoryCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Straddle.Payments.Infrastructure.Services;
using System.Threading.Tasks;
using System.Transactions;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDataContext _dataContext;
    private readonly IPublishOutbox _publisherOutbox;

    public UnitOfWork(IDataContext dataContext, IPublishOutbox publisherOutbox)
    {
        _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        _publisherOutbox = publisherOutbox ?? throw new ArgumentNullException(nameof(publisherOutbox));
    }

    public Task BeginAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        // Update the database
        IExecutionStrategy strategy = _dataContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            IDbContextTransaction transaction = await _dataContext.Database.BeginTransactionAsync(cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });

        // Publish messages
        using TransactionScope ts = new(TransactionScopeAsyncFlowOption.Enabled);
        await _publisherOutbox.SendAsync(cancellationToken);
        ts.Complete();
    }

    public Task RollbackAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}