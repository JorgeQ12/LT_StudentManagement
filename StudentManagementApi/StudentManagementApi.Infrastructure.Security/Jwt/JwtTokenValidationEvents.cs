using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using StudentManagementApi.Application.Common.Security;
using StudentManagementApi.Domain.Abstractions;

namespace StudentManagementApi.Infrastructure.Security.Jwt;

internal sealed class JwtTokenValidationEvents : JwtBearerEvents
{
    public override Task MessageReceived(MessageReceivedContext context)
    {
        context.Token = context.Request.Cookies[Cookies.AuthenticationCookie.Name];
        return Task.CompletedTask;
    }

    public override async Task TokenValidated(TokenValidatedContext context)
    {
        var subject = context.Principal?.FindFirst(SecurityClaimTypes.Subject)?.Value;
        if (!Guid.TryParse(subject, out var accountId))
        {
            context.Fail(ErrorCode.InvalidToken);
            return;
        }

        var validator = context.HttpContext.RequestServices.GetRequiredService<IAccountSessionValidator>();
        if (!await validator.IsActiveAsync(new UserAccountId(accountId), context.HttpContext.RequestAborted))
        {
            context.Fail(ErrorCode.InactiveAccount);
        }
    }

    private static class ErrorCode
    {
        public const string InvalidToken = "invalid_token";
        public const string InactiveAccount = "inactive_account";
    }
}
