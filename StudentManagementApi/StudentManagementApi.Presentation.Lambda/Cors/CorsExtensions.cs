namespace StudentManagementApi.Presentation.Lambda.Cors;

internal static class CorsExtensions
{
    public static IServiceCollection AddApiCors(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var options = configuration.GetSection(ApiCorsOptions.SectionName).Get<ApiCorsOptions>() ?? new ApiCorsOptions();
        var allowedOrigins = options.AllowedOrigins.Where(origin => !string.IsNullOrWhiteSpace(origin)).ToHashSet(StringComparer.OrdinalIgnoreCase);

        services.AddCors(cors => cors.AddPolicy(ApiCorsPolicies.Frontend, policy =>
        {
            if (environment.IsDevelopment() && options.AllowLocalhost)
                policy.SetIsOriginAllowed(origin => IsLoopbackOrigin(origin) || allowedOrigins.Contains(origin));
            else if (allowedOrigins.Count > 0)
                policy.WithOrigins([.. allowedOrigins]);

            policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials();
        }));
        return services;
    }

    private static bool IsLoopbackOrigin(string origin) => Uri.TryCreate(origin, UriKind.Absolute, out var uri) && uri.IsLoopback &&
        (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
}
