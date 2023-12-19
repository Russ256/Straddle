namespace Straddle.Payments.Infrastructure.Services;

using Straddle.Payments.Domain.Services;

internal class Publisher : IPublisher
{
    private readonly IPublishOutbox _publisherOutbox;

    public Publisher(IPublishOutbox publisherOutbox)
    {
        _publisherOutbox = publisherOutbox ?? throw new ArgumentNullException(nameof(publisherOutbox));
    }

    public void Publish(Guid correllationId, object message, DateTimeOffset? dateTime = null)
    {
        _publisherOutbox.Add(correllationId, message, dateTime);
    }
}