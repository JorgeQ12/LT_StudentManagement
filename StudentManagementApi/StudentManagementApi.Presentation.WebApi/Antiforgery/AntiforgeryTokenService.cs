using Microsoft.AspNetCore.Antiforgery;

namespace StudentManagementApi.Presentation.WebApi.Antiforgery;

internal sealed class AntiforgeryTokenService(IAntiforgery antiforgery) : IAntiforgeryTokenService
{
    public AntiforgeryTokenResponse Generate(HttpContext httpContext)
    {
        var tokens = antiforgery.GetAndStoreTokens(httpContext);
        return new(tokens.RequestToken ?? throw new InvalidOperationException(nameof(tokens.RequestToken)));
    }
}
