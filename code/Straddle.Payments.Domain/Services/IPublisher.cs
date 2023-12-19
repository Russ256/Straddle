namespace Straddle.Payments.Domain.Services;

public interface IPublisher
{
    public void Publish(Guid correllationId, object message, DateTimeOffset? dateTime = null);
}