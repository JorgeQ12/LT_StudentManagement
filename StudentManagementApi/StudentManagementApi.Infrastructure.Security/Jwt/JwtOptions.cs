namespace StudentManagementApi.Infrastructure.Security.Jwt;

public sealed class JwtOptions
{
    public const string SectionName = "Security:Jwt";
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string SigningKey { get; init; } = string.Empty;
    public int AccessTokenLifetimeMinutes { get; init; } = 15;
}
