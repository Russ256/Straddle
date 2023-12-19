namespace Straddle.Payments.Infrastructure.Services;

using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using Straddle.Payments.Domain.Messages;
using System.Text.Json;
using System.Threading;

internal class PublishOutbox : IPublishOutbox
{
    private class MessageInfo
    {
        public object? Message { get; init; }
        public string? CorrellationId { get; init; }
        public DateTimeOffset? DateTime { get; init; }
    }

    private readonly List<MessageInfo> _messages = [];
    private readonly IOptions<InfrastructureOptions> _options;
    ServiceBusSender _paymentRequestSender;
    ServiceBusSender _paymentProgessSender;

    public PublishOutbox(IOptions<InfrastructureOptions> options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));

        ServiceBusClient client = new(_options.Value.ServiceBusConnectionString);
        _paymentRequestSender = client.CreateSender("payment_request");
        _paymentProgessSender = client.CreateSender("payment_processing");

    }

    public void Add(Guid correllationId, object message, DateTimeOffset? dateTime = null)
    {
        _messages.Add(new MessageInfo() { CorrellationId = correllationId.ToString(), Message = message, DateTime = dateTime  } );
    }

    public async Task SendAsync(CancellationToken cancellationToken)
    {
        foreach (MessageInfo messageInfo in _messages)
        {
            string messageJson = JsonSerializer.Serialize(messageInfo.Message); 
            ServiceBusMessage serviceBusMessage = new(messageJson)
            {
                CorrelationId = messageInfo.CorrellationId,
                ContentType = "application/json",
            };

            ServiceBusSender serviceBusSender;
            if (messageInfo.Message is PaymentRequest)
            {
                serviceBusSender = _paymentRequestSender;
            }
            else if (messageInfo.Message is PaymentProcessing)
            {
                serviceBusSender = _paymentProgessSender;
            }
            else
            {
                throw new Exception("Unknown message type");
            }

            if (messageInfo.DateTime != null)
            {
                await serviceBusSender.ScheduleMessageAsync(serviceBusMessage, messageInfo.DateTime.Value, cancellationToken);
            }
            else
            {
                await serviceBusSender.SendMessageAsync(serviceBusMessage, cancellationToken);
            }
        }

    }
}
