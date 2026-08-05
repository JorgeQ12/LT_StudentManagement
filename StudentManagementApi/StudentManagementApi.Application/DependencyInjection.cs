using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StudentManagementApi.Application.Common.Behaviors;
using StudentManagementApi.Application.Common.Errors;

namespace StudentManagementApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        services.AddSingleton<IErrorMessageProvider, ResourceErrorMessageProvider>();
        services.AddSingleton(TimeProvider.System);
        return services;
    }
}
