namespace Straddle.Payments.Infrastructure;

using DataRepositoryCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Straddle.Payments.Domain.Data;
using Straddle.Payments.Infrastructure.Data;
using Straddle.Payments.Infrastructure.Services;
using Straddle.Payments.Infrastructure.Uow;

public static class DependencyInjection
{
    public static IServiceCollection AddPaymentsInfrastructure(this IServiceCollection services, InfrastructureOptions infrastructureOptions)
    {
        services.Configure<InfrastructureOptions>(options =>
        {
            options.DatabaseConnectionString = infrastructureOptions.DatabaseConnectionString;
            options.ServiceBusConnectionString = infrastructureOptions.ServiceBusConnectionString;
        });   

        // Add behaviors to pipeline
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

        // Data
        services.AddScoped<IPaymentHistoryUpdateRepository, PaymentHistoryUpdateRepository>();
        services.AddScoped<IPaymentUpdateRepository, PaymentUpdateRepository>();
        services.AddScoped<IPaymentReadRepository, PaymentReadRepository>();
        services.AddScoped<IDataContext, PaymentsContext>();
        services.AddScoped<IPaymentHistoryWriter, PaymentHistoryWriter>();

        // Messaging
        services.AddScoped<IPublishOutbox, PublishOutbox>();
        services.AddScoped<Domain.Services.IPublisher, Publisher>();

        services.AddPooledDbContextFactory<PaymentsContext>(options =>
        {
            options.UseSqlServer(infrastructureOptions.DatabaseConnectionString,
                s =>
                {
                 //   s.EnableRetryOnFailure();
                }
            );
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}