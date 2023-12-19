namespace Straddle.Payments.Worker;

using Azure.Messaging.ServiceBus;
using MediatR;
using Microsoft.Extensions.Options;
using Straddle.Payments.Application.Commands;
using Straddle.Payments.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

internal abstract class ServiceBusListener
{
    protected readonly IOptions<InfrastructureOptions> _options;
    protected readonly IServiceProvider ServiceProvider;
    protected ServiceBusClient? _client;
    protected ServiceBusProcessor? _processor;
    private string _subscriptionName;
    private string _topicName;

    public ServiceBusListener(IServiceProvider serviceProvider, IOptions<InfrastructureOptions> options, string topicName, string subscriptionName)
    {
        ServiceProvider = serviceProvider;
        _options = options;
        _topicName = topicName;
        _subscriptionName = subscriptionName;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _client = new(_options.Value.ServiceBusConnectionString);
        _processor = _client.CreateProcessor(_topicName, _subscriptionName, new ServiceBusProcessorOptions());
        _processor.ProcessMessageAsync += MessageHandlerAsync;
        _processor.ProcessErrorAsync += ErrorHandlerAsync;

        return _processor.StartProcessingAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return _processor!.StopProcessingAsync(cancellationToken);
    }

    protected abstract object CreateRequest(Guid correllationId);

    protected Task ErrorHandlerAsync(ProcessErrorEventArgs args)
    {
        throw args.Exception;
    }

    private async Task MessageHandlerAsync(ProcessMessageEventArgs args)
    {
        using IServiceScope scope = ServiceProvider.CreateScope();
        IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(CreateRequest(new Guid(args.Message.CorrelationId)), args.CancellationToken);
    }
}