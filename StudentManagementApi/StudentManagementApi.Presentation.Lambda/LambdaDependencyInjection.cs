using System.Text.Json.Serialization;
using StudentManagementApi.Application;
using StudentManagementApi.Infrastructure.Persistence.SqlServer;
using StudentManagementApi.Infrastructure.Persistence.SqlServer.Bootstrap;
using StudentManagementApi.Infrastructure.Security;
using StudentManagementApi.Presentation.Lambda.Antiforgery;
using StudentManagementApi.Presentation.Lambda.Cors;
using StudentManagementApi.Presentation.Lambda.Filters;
using StudentManagementApi.Presentation.Lambda.ProblemDetails;
using StudentManagementApi.Presentation.Lambda.RateLimiting;

namespace StudentManagementApi.Presentation.Lambda;

internal static class LambdaDependencyInjection
{
    public static IServiceCollection AddLambdaPresentation(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var connectionString = configuration.GetConnectionString(PersistenceConstants.ConnectionStringName)
            ?? throw new InvalidOperationException(PersistenceConstants.ConnectionStringName);

        services.AddApplication();
        services.AddSqlServerPersistence(connectionString);
        services.Configure<AdministratorBootstrapOptions>(
            configuration.GetSection(AdministratorBootstrapOptions.SectionName));
        services.AddSecurityInfrastructure(configuration);
        services.AddProblemDetails();
        services.AddExceptionHandler<ApiExceptionHandler>();
        services.AddSingleton<IApiProblemDetailsFactory, ApiProblemDetailsFactory>();
        services.AddScoped<AntiforgeryEndpointFilter>();
        services.AddScoped<IAntiforgeryTokenService, AntiforgeryTokenService>();
        services.AddApiCors(configuration, environment);
        services.AddApiRateLimiting();
        services.AddAuthorization();
        services.AddOpenApi();
        services.AddAntiforgery(options =>
        {
            options.HeaderName = "X-CSRF-TOKEN";
            options.Cookie.Name = "__Host-student_csrf";
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.Path = "/";
        });
        services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        return services;
    }
}
