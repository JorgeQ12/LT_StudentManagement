using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.Application;
using StudentManagementApi.Infrastructure.Persistence.SqlServer;
using StudentManagementApi.Infrastructure.Persistence.SqlServer.Bootstrap;
using StudentManagementApi.Infrastructure.Security;
using StudentManagementApi.Presentation.WebApi.Antiforgery;
using StudentManagementApi.Presentation.WebApi.Configuration;
using StudentManagementApi.Presentation.WebApi.Cors;
using StudentManagementApi.Presentation.WebApi.Filters;
using StudentManagementApi.Presentation.WebApi.ProblemDetails;
using StudentManagementApi.Presentation.WebApi.RateLimiting;

namespace StudentManagementApi.Presentation.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var connectionString = configuration.GetConnectionString(PersistenceConstants.ConnectionStringName)
            ?? throw new InvalidOperationException(PersistenceConstants.ConnectionStringName);
        services.AddApplication();
        services.AddSqlServerPersistence(connectionString);
        services.AddSecurityInfrastructure(configuration);
        services.AddProblemDetails();
        services.AddExceptionHandler<ApiExceptionHandler>();
        services.AddSingleton<IApiProblemDetailsFactory, ApiProblemDetailsFactory>();
        services.AddScoped<InvalidModelStateResponseFactory>();
        services.AddScoped<ApiResultFilter>();
        services.AddScoped<ApiAntiforgeryFilter>();
        services.AddScoped<IAntiforgeryTokenService, AntiforgeryTokenService>();
        services.AddApiCors(configuration, environment);
        services.AddAntiforgery(options =>
        {
            options.HeaderName = "X-CSRF-TOKEN";
            options.Cookie.Name = "__Host-student_csrf";
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.Path = "/";
        });
        services.AddApiRateLimiting();
        services.AddOpenApi();
        services.AddOptions<AdministratorBootstrapOptions>().Bind(configuration.GetSection(AdministratorBootstrapOptions.SectionName))
            .Validate(options => string.IsNullOrWhiteSpace(options.Email) == string.IsNullOrWhiteSpace(options.Password),
                AdministratorBootstrapOptions.SectionName)
            .Validate(options => string.IsNullOrWhiteSpace(options.Password) || options.Password.Length >= 12,
                AdministratorBootstrapOptions.SectionName)
            .ValidateOnStart();
        services.AddControllers(options =>
            {
                options.Filters.AddService<ApiResultFilter>();
                options.Filters.AddService<ApiAntiforgeryFilter>();
            })
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        services.Configure<ApiBehaviorOptions>(options => options.InvalidModelStateResponseFactory = context =>
            context.HttpContext.RequestServices.GetRequiredService<InvalidModelStateResponseFactory>().Create(context));
        return services;
    }
}
