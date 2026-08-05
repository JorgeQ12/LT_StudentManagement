using Microsoft.AspNetCore.Diagnostics;
using StudentManagementApi.Application.Common.Errors;

namespace StudentManagementApi.Presentation.WebApi.ProblemDetails;

internal static class ApiStatusCodeHandler
{
    public static Task HandleAsync(StatusCodeContext context)
    {
        var factory = context.HttpContext.RequestServices.GetRequiredService<IApiProblemDetailsFactory>();
        var code = context.HttpContext.Response.StatusCode switch
        {
            StatusCodes.Status401Unauthorized => ErrorCode.Unauthorized,
            StatusCodes.Status403Forbidden => ErrorCode.Forbidden,
            StatusCodes.Status404NotFound => ErrorCode.EndpointNotFound,
            StatusCodes.Status405MethodNotAllowed => ErrorCode.MethodNotAllowed,
            StatusCodes.Status415UnsupportedMediaType => ErrorCode.UnsupportedMediaType,
            StatusCodes.Status429TooManyRequests => ErrorCode.RateLimitExceeded,
            StatusCodes.Status503ServiceUnavailable => ErrorCode.ServiceUnavailable,
            _ => ErrorCode.ValidationFailed
        };
        var details = factory.Create(code, context.HttpContext.Response.StatusCode, context.HttpContext);
        return context.HttpContext.Response.WriteAsJsonAsync(details);
    }
}
