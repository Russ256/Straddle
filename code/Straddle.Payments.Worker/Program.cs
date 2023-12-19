namespace Straddle.Payments.Worker;

using Straddle.Payments.Api.Security;
using Straddle.Payments.Application;
using Straddle.Payments.Domain.Services;
using Straddle.Payments.Infrastructure;

public class Program
{
    public static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddPaymentsApplication();
        InfrastructureOptions infrastructureOptions = new();
        builder.Configuration.Bind("Infrastructure", infrastructureOptions);
        builder.Services.AddPaymentsInfrastructure(infrastructureOptions);
        builder.Services.AddSingleton<IUserProvider, UserProvider>();

        builder.Services.AddHostedService<PaymentRequestListener>();
        builder.Services.AddHostedService<PaymentProgressListener>();

        IHost host = builder.Build();
        host.Run();
    }
}