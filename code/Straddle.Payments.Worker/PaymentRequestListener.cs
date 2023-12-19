namespace Straddle.Payments.Worker;

using Microsoft.Extensions.Options;
using Straddle.Payments.Application.Commands;
using Straddle.Payments.Infrastructure;
using System;

internal class PaymentRequestListener : ServiceBusListener, IHostedService
{
    public PaymentRequestListener(IServiceProvider serviceProvider, IOptions<InfrastructureOptions> options)
           : base(serviceProvider, options, "payment_request", "process")
    {
    }

    protected override object CreateRequest(Guid correllationId)
    {
        return new ProcessPaymentRequest(correllationId);
    }
}