namespace Straddle.Payments.Worker;

using Microsoft.Extensions.Options;
using Straddle.Payments.Application.Commands;
using Straddle.Payments.Infrastructure;
using System;

internal class PaymentProgressListener : ServiceBusListener, IHostedService
{
    public PaymentProgressListener(IServiceProvider serviceProvider, IOptions<InfrastructureOptions> options)
        : base(serviceProvider, options, "payment_processing", "progress_check")
    {
    }

    protected override object CreateRequest(Guid correllationId)
    {
        return new ProcessPaymentCheckRequest(correllationId);
    }
}