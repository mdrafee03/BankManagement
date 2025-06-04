using BankManagement.Application.Abstractions.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BankManagement.Application;

public static class ServiceExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(ServiceExtension).Assembly);
            configuration.AddOpenBehavior(typeof(ValidatorBehaviors.ValidationBehavior<,>));
        });
        services.AddValidatorsFromAssembly(typeof(ServiceExtension).Assembly);
    }
}