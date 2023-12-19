namespace Straddle.Payments.Application;

using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Straddle.Application.Behaviors;

public static class DependencyInjection
{
    public static IServiceCollection AddPaymentsApplication(this IServiceCollection services)
    {
        // Validators
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));

        // MedaitR handlers
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // MedaitR request pipeline - Remember to add in order of execution!
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}