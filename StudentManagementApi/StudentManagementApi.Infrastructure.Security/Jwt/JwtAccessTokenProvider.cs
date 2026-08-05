using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using StudentManagementApi.Application.Common.Security;
using StudentManagementApi.Domain.Accounts;

namespace StudentManagementApi.Infrastructure.Security.Jwt;

internal sealed class JwtAccessTokenProvider(IOptions<JwtOptions> options) : IAccessTokenProvider
{
    private readonly JwtOptions _options = options.Value;

    public AccessToken Create(UserAccount account, DateTimeOffset issuedAtUtc)
    {
        var expiresAtUtc = issuedAtUtc.AddMinutes(_options.AccessTokenLifetimeMinutes);
        var claims = new Dictionary<string, object>
        {
            [SecurityClaimTypes.Subject] = account.Id.Value.ToString(),
            [SecurityClaimTypes.Role] = account.Role.ToString(),
            [SecurityClaimTypes.JwtId] = Guid.NewGuid().ToString()
        };

        if (account.StudentId.HasValue)
        {
            claims[SecurityClaimTypes.StudentId] = account.StudentId.Value.Value.ToString();
        }

        var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey)), SecurityAlgorithms.HmacSha256);
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            Claims = claims,
            IssuedAt = issuedAtUtc.UtcDateTime,
            NotBefore = issuedAtUtc.UtcDateTime,
            Expires = expiresAtUtc.UtcDateTime,
            SigningCredentials = credentials
        };

        return new AccessToken(new JsonWebTokenHandler().CreateToken(descriptor), expiresAtUtc);
    }
}
