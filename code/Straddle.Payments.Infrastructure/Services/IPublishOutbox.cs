namespace Straddle.Payments.Infrastructure.Services;
using System.Threading;

public interface IPublishOutbox
{
    public void Add(Guid correllationId, object message, DateTimeOffset? dateTime = null);

    public Task SendAsync(CancellationToken cancellationToken);
}
