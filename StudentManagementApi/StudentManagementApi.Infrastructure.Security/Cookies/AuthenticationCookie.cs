using Microsoft.AspNetCore.Http;

namespace StudentManagementApi.Infrastructure.Security.Cookies;

public static class AuthenticationCookie
{
    public const string Name = "__Host-student_access_token";

    public static CookieOptions CreateOptions(DateTimeOffset expiresAtUtc) => new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Path = "/",
        Expires = expiresAtUtc
    };

    public static CookieOptions CreateExpiredOptions() => CreateOptions(DateTimeOffset.UnixEpoch);
}
