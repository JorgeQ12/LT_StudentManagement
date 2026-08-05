using System.Threading.RateLimiting;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Presentation.WebApi.ProblemDetails;

namespace StudentManagementApi.Presentation.WebApi.RateLimiting;

internal static class RateLimitingExtensions
{
    public static IServiceCollection AddApiRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddPolicy(RateLimitPolicies.Registration, httpContext => CreatePartition(httpContext, 5));
            options.AddPolicy(RateLimitPolicies.Login, httpContext => CreatePartition(httpContext, 10));
            options.OnRejected = async (context, cancellationToken) =>
            {
                var factory = context.HttpContext.RequestServices.GetRequiredService<IApiProblemDetailsFactory>();
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.HttpContext.Response.WriteAsJsonAsync(
                    factory.Create(ErrorCode.RateLimitExceeded, StatusCodes.Status429TooManyRequests, context.HttpContext), cancellationToken);
            };
        });
        return services;
    }

    private static RateLimitPartition<string> CreatePartition(HttpContext httpContext, int permitLimit) =>
        RateLimitPartition.GetFixedWindowLimiter(httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown-client", _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = permitLimit,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0,
            AutoReplenishment = true
        });
}
