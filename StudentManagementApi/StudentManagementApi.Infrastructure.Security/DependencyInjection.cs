using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StudentManagementApi.Application.Common.Security;
using StudentManagementApi.Infrastructure.Security.CurrentUser;
using StudentManagementApi.Infrastructure.Security.Jwt;
using StudentManagementApi.Infrastructure.Security.Passwords;

namespace StudentManagementApi.Infrastructure.Security;

public static class DependencyInjection
{
    public static IServiceCollection AddSecurityInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
            ?? throw new InvalidOperationException(JwtOptions.SectionName);
        Validate(jwtOptions);

        services.AddSingleton(Microsoft.Extensions.Options.Options.Create(jwtOptions));
        services.AddHttpContextAccessor();
        services.AddScoped<IPasswordHasher, AspNetPasswordHasher>();
        services.AddScoped<IAccessTokenProvider, JwtAccessTokenProvider>();
        services.AddScoped<ICurrentUser, HttpCurrentUser>();
        services.AddScoped<JwtTokenValidationEvents>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.MapInboundClaims = false;
            options.EventsType = typeof(JwtTokenValidationEvents);
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtOptions.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                NameClaimType = SecurityClaimTypes.Subject,
                RoleClaimType = SecurityClaimTypes.Role
            };
        });
        services.AddAuthorization();
        return services;
    }

    private static void Validate(JwtOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Issuer) || string.IsNullOrWhiteSpace(options.Audience) || options.SigningKey.Length < 32 ||
            options.AccessTokenLifetimeMinutes != 15)
        {
            throw new InvalidOperationException(JwtOptions.SectionName);
        }
    }
}
